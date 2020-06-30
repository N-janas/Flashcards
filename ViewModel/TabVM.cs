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
        public TabVM(Model model)
        {
            LangTabVM = new LanguagesTabViewModel(model);
            // FcardTabVM = new FlashCardTabViewModel(model);
        }
        #endregion

    }
}
