using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using BaseClasses;
    using System.Diagnostics;
    using FlashCards.Model;
    using System.Collections.ObjectModel;
    using DAL.Encje;
    using System.Windows.Input;

    class LanguageTrainingVM : BaseViewModel
    {
        #region Pola Prywatne
        private Model model = null;
        private Word question;
        private Word answer;
        private List<Word> questions = null;
        private List<Word> answers = null;
        private List<FrontBack> _frontBack = null;
        private List<WordKnowledge> performance = null;
        private sbyte? user = null;
        private Language _translation;
        private string otherTranslations;
        // Parametr do iteracji (nieskończona sesja = użytkownik stopuje)
        private int iter = 0;
        // Domyślny stan okna
        private bool isUserGuessing = true;
        private bool isUserRating = false;
        #endregion

        #region Własności
        public string OtherTranslations
        {
            get { return otherTranslations; }
            set { otherTranslations = value; onPropertyChanged(nameof(OtherTranslations)); }
        }

        public Word Question
        {
            get { return question; }
            set { question = value; onPropertyChanged(nameof(Question)); }
        }

        public Word Answer
        {
            get { return answer; }
            set { answer = value; onPropertyChanged(nameof(Answer)); }
        }

        public List<Word> Questions
        {
            get { return questions; }
            set { questions = value; onPropertyChanged(nameof(Questions)); }
        }

        public List<Word> Answers
        {
            get { return answers; }
            set { answers = value; onPropertyChanged(nameof(Answers)); }
        }

        public List<FrontBack> FrontBacks
        {
            get { return _frontBack; }
            set { _frontBack = value; onPropertyChanged(nameof(FrontBacks)); }
        }

        public List<WordKnowledge> Performance
        {
            get { return performance; }
            set { performance = value; onPropertyChanged(nameof(Performance)); }
        }

        public bool IsUserGuessing
        {
            get { return isUserGuessing; }
            set { isUserGuessing = value; onPropertyChanged(nameof(isUserGuessing)); }
        }

        public bool IsUserRating
        {
            get { return isUserRating; }
            set { isUserRating = value; onPropertyChanged(nameof(IsUserRating)); }
        }
        public string Title { get; set; }
        #endregion

        #region Konstruktory
        public LanguageTrainingVM() 
        {
            // Initialize only
        }

        public LanguageTrainingVM(Model model, List<Word> questions, List<Word> answers, List<FrontBack> frontBack, sbyte? id_user, string langA, Language langB)
        {
            this.model = model;
            Questions = questions;
            Answers = answers;
            _frontBack = frontBack;
            // Inicjalizacja nowej listy z levelem
            // będzie ona uzupełniana i wysyłana do aktualizacji 
            // czestotliwości pojawiania się słów po zakończonej sesji treningowej
            Performance = new List<WordKnowledge>();
            user = id_user;
            Title = langA+" -> "+langB.LangName;
            _translation = langB;
            //foreach (var w in Questions)
            //{
            //    Console.WriteLine(w);
            //}
            GetNewWord();

        }
        #endregion

        #region Metody
        private WordKnowledge SaveProgres(sbyte factor)
        {
            // Znajdź odpowiednią krotke
            foreach (var fb in FrontBacks)
            {
                // Jeden zawsze nie spełniony, zapisujemy języki z mniejszego na większy i takie sa trzymane w FrontBacks
                // ale nie jesteśmy pewni w której kolejności się uczy teraz user więc sprawdzamy obie
                if( (fb.Front == Question && fb.Back == Answer) || (fb.Back == Question && fb.Front == Answer))
                {
                    // I zwróć ją jako odpowiednią w formacie do bazy
                    // Sprawdza czy jest w przedziale 0-127
                    factor += fb.Knowledge;
                    if (factor > 127 || factor < 0)
                    {
                        return new WordKnowledge(fb.Front.Id, fb.Back.Id, (sbyte)user, fb.Knowledge);
                    }
                    else
                    {
                        // Zaktualizuj również stan w obecnym oknie
                        FrontBacks[FrontBacks.IndexOf(fb)].Knowledge = factor;
                        return new WordKnowledge(fb.Front.Id, fb.Back.Id, (sbyte)user, factor);
                    }     
                }
            }
            return null;
        }

        private static Random random = new Random();
        private List<Word> Shuffle(List<Word> list)
        {
            for (int n = list.Count - 1; n > 1; n--)
            {
                int rng = random.Next(n + 1);
                Word value = list[rng];
                list[rng] = list[n];
                list[n] = value;
            }
            return list;
        }

        private void GetNewWord()
        {
            // Wpisanie przykładu do zgadnięcia
            Question = Questions[iter++];
            Answer = FindAnswerByGUID(Question);

            // Jeśli przekraczamy poza tabele, wróć z indeksem i przelosuj przykłady
            if (iter > Questions.Count-1)
            {
                iter = 0;
                Questions = Shuffle(Questions);
            }
        }

        private List<Word> FindOtherMeanings(Word origin)
        {
            return model.PassOtherTranslations(origin, _translation);
        }

        private Word FindAnswerByGUID(Word q)
        {
            // Znajdź pasującą odpowiedź
            Word ans = null;
            // Przeszukuj słowa w odpowiedziach do znalezienia pasującego GUIDA
            foreach (var w in Answers)
                if (w.GUID == q.GUID)
                {
                    ans = w;
                    break;
                } 
            return ans;
        }

        #endregion

        #region Komendy

        private ICommand showAnswer = null;

        public ICommand ShowAnswer
        {
            get
            {
                if(showAnswer == null)
                {
                    showAnswer = new RelayCommand(
                        arg =>
                        {
                            // Jeśli są inne tłumaczenia to znajdź 
                            List<Word> others = FindOtherMeanings(Question);
                            if (others.Any())
                            {
                                // I wyświetl
                                OtherTranslations = "Inne : ";
                                foreach (var other in others)
                                {
                                    OtherTranslations += other + ", ";
                                }
                            }
                            else
                                OtherTranslations = "";

                            // Zmiana stanu na ocenianie
                            IsUserGuessing = false;
                            IsUserRating = true;
                            //
                        },
                        arg => true
                        );
                }

                return showAnswer;
            }
        }


        private ICommand grantMinusOne = null;

        public ICommand GrantMinusOne
        {
            get
            {
                if (grantMinusOne == null)
                {
                    grantMinusOne = new RelayCommand(
                        arg =>
                        {
                            model.UpdateWordKnowledge(SaveProgres(-1));
                            GetNewWord();
                            // Zmiana stanu na zgadywanie
                            IsUserGuessing = true;
                            IsUserRating = false;
                            //
                        },
                        arg => true
                        );
                }

                return grantMinusOne;
            }
        }

        private ICommand grantPlusOne = null;

        public ICommand GrantPlusOne
        {
            get
            {
                if (grantPlusOne == null)
                {
                    grantPlusOne = new RelayCommand(
                        arg =>
                        {
                            model.UpdateWordKnowledge(SaveProgres(1));
                            GetNewWord();
                            // Zmiana stanu na zgadywanie
                            IsUserGuessing = true;
                            IsUserRating = false;
                            //
                        },
                        arg => true
                        );
                }

                return grantPlusOne;
            }
        }

        private ICommand grantPlusThree = null;

        public ICommand GrantPlusThree
        {
            get
            {
                if (grantPlusThree == null)
                {
                    grantPlusThree = new RelayCommand(
                        arg =>
                        {
                            model.UpdateWordKnowledge(SaveProgres(3));
                            GetNewWord();
                            // Zmiana stanu na zgadywanie
                            IsUserGuessing = true;
                            IsUserRating = false;
                            //
                        },
                        arg => true
                        );
                }

                return grantPlusThree;
            }
        }

        private ICommand goBack = null;

        public ICommand GoBack
        {
            get
            {
                if (goBack == null)
                {
                    goBack = new RelayCommand(
                        arg =>
                        {
                            // Wrzucamy false aby wrócić do zakładki języków
                            Mediator.Notify("BackFromTrain1", false);
                        },
                        arg => true
                        );
                }

                return goBack;
            }
        }
        #endregion
    }
}
