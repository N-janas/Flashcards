using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using FlashCards.Model;
    class TabVM : BaseViewModel
    {
        // Stan służący za określanie do jakiego widoku
        // zakładki mamy powrócić (fiszki / języki)
        private bool isSelectedFlipCardTab = false;
        public bool IsSelectedFlipCardTab
        {
            get { return isSelectedFlipCardTab; }
            set { isSelectedFlipCardTab = value; onPropertyChanged(nameof(isSelectedFlipCardTab)); }
        }

        private sbyte? loggedUser = null;

        public sbyte? LoggedUser
        {
            get { return loggedUser; }
            set { loggedUser = value; onPropertyChanged(nameof(LoggedUser)); }
        }
        #region ViewModele Zakładek

        private LanguagesTabViewModel langTabVM = null;
        public LanguagesTabViewModel LangTabVM
        {
            get { return langTabVM; }
            set { langTabVM = value; onPropertyChanged(nameof(LangTabVM)); }
        }

        private FlaszkardsViewModel fcardTabVM = null;

        public FlaszkardsViewModel FcardTabVM
        {
            get { return fcardTabVM; }
            set { fcardTabVM = value; onPropertyChanged(nameof(FcardTabVM)); }
        }

        #endregion

        #region Konstruktory

        public TabVM()
        {
            // Initialize only
        }
        public TabVM(Model model, sbyte? user)
        {
            LoggedUser = user;
            LangTabVM = new LanguagesTabViewModel(model, user);
            FcardTabVM = new FlaszkardsViewModel(model, user);
        }

        #endregion

    }
}
