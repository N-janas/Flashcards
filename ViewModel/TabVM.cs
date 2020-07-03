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

        // private FlashCardTabViewModel fcardTabVM = null;

        //public FlashCardTabViewModel FcardTabVM
        //{
        //    get { return fcardTabVM; }
        //    set { fcardTabVM = value; onPropertyChanged(nameof(FcardTabVM)); }
        //}

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
            // FcardTabVM = new FlashCardTabViewModel(model, user);
        }

        #endregion

    }
}
