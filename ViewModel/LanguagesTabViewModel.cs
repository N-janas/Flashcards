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

    class LanguagesTabViewModel : BaseViewModel
    {
        #region Prywatne składowe
        private Model model = null;
        private sbyte? loggedUser = null;
        private ObservableCollection<Language> langCollection = new ObservableCollection<Language>();
        private List<string> difficulties = new List<string>();

        private Language selectedLangZ = null;
        private Language selectedLangNa = null;
        private string selectedDifficulty = null;
        #endregion

        #region Własności
        public string SelectedDifficulty
        {
            get { return selectedDifficulty; }
            set { selectedDifficulty = value; onPropertyChanged(nameof(SelectedDifficulty)); }
        }

        public Language SelectedLangNa
        {
            get { return selectedLangNa; }
            set { selectedLangNa = value; onPropertyChanged(nameof(SelectedLangNa)); }
        }

        public Language SelectedLangZ
        {
            get { return selectedLangZ; }
            set { selectedLangZ = value; onPropertyChanged(nameof(SelectedLangZ)); }
        }

        public ObservableCollection<Language> LangCollection
        {
            get { return langCollection; }
            set { langCollection = value; onPropertyChanged(nameof(LangCollection)); }
        }

        public List<string> Difficulties
        {
            get { return difficulties; }
            set { difficulties = value; onPropertyChanged(nameof(Difficulties)); }
        }

        public sbyte? LoggedUser
        {
            get { return loggedUser; }
            set { loggedUser = value; onPropertyChanged(nameof(LoggedUser)); }
        }

        #endregion

        #region Konstruktory
        // Trzymany w MainVM
        public LanguagesTabViewModel(Model model, sbyte? user)
        {
            LoggedUser = user;
            this.model = model;
            Difficulties = this.model.PassDifficulties();
            LangCollection = this.model.Langs;
        }

        public LanguagesTabViewModel()
        {
            // Initialize only
        }
        #endregion

        #region Metody

        private bool Match(Word wordA, Word wordB, WordKnowledge wordKnowledge)
        {
            if (wordA.GUID == wordB.GUID && wordKnowledge.Id_word_back == wordB.Id && wordKnowledge.Id_word_front == wordA.Id) 
                return true;
            return false;
        }
        public List<FrontBack> CreateFrontBack(List<Word> langA, List<Word> langB, List<WordKnowledge> wordKnowledges)
        {
            List<FrontBack> frontBackList = new List<FrontBack>();

            // Szukamy odpowiadających słów
            foreach (Word wordA in langA)
            {
                foreach (Word wordB in langB)
                {
                    if (wordA.GUID == wordB.GUID)
                    {
                        // Inicjujemy kwLevel pary zerem (jeśli już istnieje nadpisz inną wartością)
                        sbyte knowledge = 0;
                        foreach (WordKnowledge wordKnowledge in wordKnowledges)
                        {
                            if (Match(wordA, wordB, wordKnowledge)) knowledge = wordKnowledge.Knowledge;
                        }
                        frontBackList.Add(new FrontBack(wordA, wordB, knowledge));
                        break;
                    }
                }
            }

            return frontBackList;
        }

        private void FindMinAndMaxKnowledge(List<FrontBack> frontBackList, out sbyte min, out sbyte max)
        {
            max = 0;
            min = 127;

            foreach (FrontBack frontBack in frontBackList)
            {
                if (frontBack.Knowledge > max) max = frontBack.Knowledge;
                if (frontBack.Knowledge < min) min = frontBack.Knowledge;
            }
        }

        private static Random random = new Random();

        public List<Word> Shuffle(List<Word> list)
        {
            for (int n = list.Count-1; n > 1; n--)
            {
                int rng = random.Next(n + 1);
                Word value = list[rng];
                list[rng] = list[n];
                list[n] = value;
            }

            return list;
        }

        public void SplitWords(List<Word> allWords, sbyte idFront, sbyte idBack, out List<Word> langA, out List<Word> langB)
        {
            // Podzielenie na słów na języki 
            langA = new List<Word>();
            langB = new List<Word>();

            foreach(Word word in allWords)
            {
                if (word.Id_lang == idFront)
                    langA.Add(word);
                else if (word.Id_lang == idBack)
                    langB.Add(word);
                else
                    Debug.WriteLine("Data passed to 'CreateQueue' method appeared to be incorrect");
            }
        }

        public List<Word> CreateQueue(
            List<Word> allWords, List<WordKnowledge> wordKnowledges, sbyte idFront, sbyte idBack,
            out List<FrontBack> fBL, out List<Word> translations
            )
        {
            // Określenie bazy i tłumaczenia 
            sbyte origin = idFront;
            sbyte translation = idBack;
            // Określenia pierwszego i drugiego parametru krotki knowledgeLevel  (języki z mniejszego -> na większy) 
            // oraz tłumaczenie np Ang->Pol = Pol->Ang (ten sam KnowledgeLevel) dlatego nie chcemy trzymać 2 krotek tylko zawsze
            // z mniejszego języka na większy (id)
            idFront = Math.Min(origin, translation);
            idBack = Math.Max(origin, translation);

            SplitWords(allWords, idFront, idBack, out List<Word> langA, out List<Word> langB);
            if (origin < translation)
                translations = langB;
            else
                translations = langA;
            // Tworzenie uproszczonych obiektów przodu i tyłu karty
            List<FrontBack> frontBackList = CreateFrontBack(langA, langB, wordKnowledges);
            fBL = frontBackList;

            FindMinAndMaxKnowledge(frontBackList, out sbyte maxKnowledge, out sbyte minKnowledge);

<<<<<<< HEAD
            sbyte difference = minKnowledge;
            difference -= maxKnowledge;

            sbyte tempDifference = difference;
            sbyte differenceDecreaser = 1;
            while(tempDifference > 5)
            {
                tempDifference /= 2;
                differenceDecreaser += 1;
            }
                
=======
            sbyte difference = maxKnowledge;
            difference -= minKnowledge;
>>>>>>> 699e4a430fa9cb8da235e4e4893b84166a30c470

            // Tworzenie kolejki z której będą losowane słowa
            List<Word> queue = new List<Word>();

            // Utworzenie odpowiedniej liczby duplikatów, zwiększającej prawdopodobieństwo
            // na wylosowanie słówek mniej znanych (wyznacznik knowledgeLevel)
            foreach (FrontBack frontBack in frontBackList)
            {
                sbyte ownDifference = difference;
                ownDifference /= differenceDecreaser;
                sbyte repetitions = maxKnowledge;
                repetitions += ownDifference;
                repetitions -= frontBack.Knowledge;
                repetitions += 1;

                if (repetitions > 5)
                    repetitions = 5;

                if (repetitions <= 0)
                    repetitions = 1;

                for (int i = 0; i < repetitions; i++)
                {
                    // Z mniejszego id języka na większy
                    if (origin < translation)
                        queue.Add(frontBack.Front);
                    else
                        queue.Add(frontBack.Back);

                }

            }

            queue = Shuffle(queue);

            foreach (Word item in queue)
            {
                Debug.WriteLine(item);
            }

            return queue;
        }

        public bool ValidateString(string str)
        {
            foreach(char c in str)
            {
                if (c == ';' || c == '\'' || c == '-')
                    return false;
            }
            return true;
        }

        #endregion

        #region Komendy

        private ICommand train = null;

        public ICommand Train
        {
            get
            {
                if (train == null)
                {
                    train = new RelayCommand(
                        arg =>
                        {
                            // Check if all items choosed
                            if (SelectedLangZ != null && SelectedLangNa != null && SelectedDifficulty != null)
                            {
                                // Check if Z and Na isnt the same 
                                // Id check bo nie ma override Equals
                                if (SelectedLangZ.Id != SelectedLangNa.Id)  
                                {
                                    List<List<TrainData>> daneTreningowe = new List<List<TrainData>>();

                                    // Wyznaczenie kolejki pytań (z duplikatami)
                                    List<Word> questions = CreateQueue(
                                            model.PassWordCollection(SelectedLangZ.Id, SelectedLangNa.Id, SelectedDifficulty),
                                            model.PassUserPerformance(LoggedUser, SelectedLangZ.Id, SelectedLangNa.Id),
                                            SelectedLangZ.Id,
                                            SelectedLangNa.Id,
                                            out List<FrontBack> fBL,
                                            out List<Word> translations
                                            );

                                    // Dodanie danych treningowych (pytania, odpowiedzi) oraz zbioru uproszczonych krotek kwLevel
                                    daneTreningowe.Add(questions.Cast<TrainData>().ToList());
                                    daneTreningowe.Add(translations.Cast<TrainData>().ToList());
                                    daneTreningowe.Add(fBL.Cast<TrainData>().ToList());

                                    Mediator.Notify("TrainLangs", daneTreningowe);
                                }
                                else
                                    System.Windows.MessageBox.Show("Wybierz różne języki");
                            }
                            else
                                System.Windows.MessageBox.Show("Wybierz każdy z parametrów");
                        },
                        arg => true
                        );
                }

                return train;
            }
        }

        private ICommand logout = null;

        public ICommand Logout
        {
            get
            {
                if (logout == null)
                {
                    logout = new RelayCommand(
                        arg =>
                        {
                            // Powrót okna
                            Mediator.Notify("Logout", "");
                        },
                        arg => true
                        );
                }

                return logout;
            }
        }
        #endregion
    }
}
