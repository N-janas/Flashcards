using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace FlashCards.ViewModel
{
    using Model;
    class MainViewModel : BaseViewModel
    {
        #region Własności
        Model model = new Model();

        // Aktualny widoczny stan
        private BaseViewModel _actualViewModel = null;
        public BaseViewModel ActualViewModel
        {
            get { return _actualViewModel; }
            set { _actualViewModel = value; onPropertyChanged(nameof(ActualViewModel)); }
        }

        // Lista Vm'ów
        List<BaseViewModel> vms = new List<BaseViewModel>();
        public List<BaseViewModel> Vms
        {
            get
            {
                if (vms == null)
                    vms = new List<BaseViewModel>();
                return vms;
            }
        }

        private LoggingPageViewModel loginPage = null;
        public LoggingPageViewModel LoginPage
        {
            get { return loginPage; }
            set { loginPage = value; onPropertyChanged(nameof(LoginPage)); }
        }

        private TabVM tabPage = null;
        public TabVM TabPage
        {
            get { return tabPage; }
            set { tabPage = value; onPropertyChanged(nameof(TabPage)); }
        }
        // Będę jeszcza dwa widoki z treningów (języki i fcard)
        #endregion

        public MainViewModel()
        {
            LoginPage = new LoggingPageViewModel(model);
            TabPage = new TabVM(model);

            Vms.Add(LoginPage);
            Vms.Add(TabPage);

            this._actualViewModel = LoginPage; // Starter VM

            Mediator.Subscribe("GoToTabsPage", GoToTabsScreen);
            // Mediator logout 
            // Mediator train

        }

        #region Metody
        public void ChangeViewModel(BaseViewModel viewModel)
        {
            if (!Vms.Contains(viewModel))
                Vms.Add(viewModel);

            ActualViewModel = Vms.FirstOrDefault(vm => vm == viewModel); // hmm ?
        }

        private void GoToTabsScreen(object obj)
        {
            // Nadanie Usera po zalogowaniu
            TabPage.LangTabVM.LoggedUser = obj as sbyte?;

            // Przełączenie contentu
            ChangeViewModel(Vms[1]);
        }
        
        #endregion
    }
}
