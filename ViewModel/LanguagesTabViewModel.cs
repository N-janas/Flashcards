using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using BaseClasses;
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
        public LanguagesTabViewModel(Model model)
        {
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

        // TMP
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
                            // Wylogowanie
                            LoggedUser = null;
                            // Powrót okna
                            Console.WriteLine();
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
