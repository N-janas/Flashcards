using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using FlashCards.Model;
    using System.Collections.ObjectModel;
    using DAL.Encje;
    class LanguagesTabViewModel : BaseViewModel
    {
        #region Prywatne składowe
        private Model model = null;
        private sbyte? loggedUser = null;
        private ObservableCollection<Language> langCollection = new ObservableCollection<Language>();
        private List<string> difficulties = new List<string>();
        private Language selectedLangZ;
        private Language selectedLangNa;
        private string selectedDifficulty;
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
        }

        public LanguagesTabViewModel()
        {
            // Initialize only
        }
        #endregion

        #region Metody

        #endregion

        #region Komendy

        #endregion
    }
}
