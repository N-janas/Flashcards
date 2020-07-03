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
        private string question;
        private string answer;
        private List<Word> questions = null;
        private List<Word> answers = null;
        private List<FrontBack> _frontBack = null;
        private List<WordKnowledge> performance = null;
        private sbyte? user = null;
        #endregion

        #region Własności
        public string Question
        {
            get { return question; }
            set { question = value; onPropertyChanged(nameof(Question)); }
        }

        public string Answer
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
        #endregion

        #region Konstruktory
        public LanguageTrainingVM() 
        {
            // Initialize only
        }

        public LanguageTrainingVM(Model model, List<Word> questions, List<Word> answers, List<FrontBack> fronBack, sbyte? id_user)
        {
            this.model = model;
            Questions = questions;
            Answers = answers;
            _frontBack = fronBack;
            // Inicjalizacja nowej listy z levelem
            // będzie ona uzupełniana i wysyłana do aktualizacji 
            // czestotliwości pojawiania się słów po zakończonej sesji treningowej
            Performance = new List<WordKnowledge>();
            user = id_user;
            question = "";
            answer = "";
        }
        #endregion

        #region Metody
        private void Loaded_Test(Object sender, EventArgs e)
        {
            Console.WriteLine("TEST");
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
                            // check zeby nie wyjsc za 0
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

                        },
                        arg => true
                        );
                }

                return grantPlusThree;
            }
        }
        #endregion
    }
}
