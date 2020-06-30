using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using FlashCards.Model;
    class LanguagesTabViewModel : BaseViewModel
    {
        private Model model = null;

        private sbyte? loggedUser = null;

        public sbyte? LoggedUser
        {
            get { return loggedUser; }
            set { loggedUser = value; onPropertyChanged(nameof(LoggedUser)); }
        }

        #region Konstruktory
        // Trzymany w MainVM
        public LanguagesTabViewModel(Model model)
        {
            this.model = model;
        }

        public LanguagesTabViewModel()
        {
            // Initialize only
        }
        #endregion
    }
}
