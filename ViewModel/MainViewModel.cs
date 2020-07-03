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
    using DAL.Encje;
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

        private LanguageTrainingVM langTrain = null;
        public LanguageTrainingVM LangTrain
        {
            get { return langTrain; }
            set { langTrain = value; onPropertyChanged(nameof(LangTrain)); }
        }
        // Będę jeszcza dwa widoki z treningów (języki i fcard)
        #endregion

        public MainViewModel()
        {
            // Przypisanie domyślnych widoków
            LoginPage = new LoggingPageViewModel(model);
            TabPage = new TabVM();
            LangTrain = new LanguageTrainingVM();

            // Wpisanie ich na miejsca w liście
            Vms.Add(LoginPage);
            Vms.Add(TabPage);
            Vms.Add(LangTrain);

            this._actualViewModel = LoginPage; // Starter VM

            // Dodanie funkcji zmiany na odpowiednie vm'y dla mediatora 
            // Login
            Mediator.Subscribe("GoToTabsPage", GoToTabsScreen);
            // Logout
            Mediator.Subscribe("Logout", BackToLoginPage);
            // Mediator train1
            Mediator.Subscribe("TrainLangs", TrainPredefinedLangs);
            // Mediator GoBack from train1
            Mediator.Subscribe("BackFromTrain1", GoBackFromTrainLang);
            // Mediator train2

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
            TabPage = new TabVM(model, (sbyte?)obj);

            // Przełączenie contentu na okno zakładek
            ChangeViewModel(Vms[1]);
        }

        private void BackToLoginPage(object obj)
        {
            // Powrót do ekranu logowania
            LoginPage = new LoggingPageViewModel(model);
            ChangeViewModel(Vms[0]);
        }

        private void TrainPredefinedLangs(object obj)
        {
            List<List<TrainData>> daneTreningowe = obj as List<List<TrainData>>;

            // Przekazanie nowych danych treningowych nowemu oknu
            LangTrain = new LanguageTrainingVM(
                model,
                daneTreningowe[0].Cast<Word>().ToList(),
                daneTreningowe[1].Cast<Word>().ToList(),
                daneTreningowe[2].Cast<FrontBack>().ToList(),
                TabPage.LoggedUser,
                TabPage.LangTabVM.SelectedLangZ.LangName,
                TabPage.LangTabVM.SelectedLangNa
                );
            ChangeViewModel(Vms[2]);
        }

        private void GoBackFromTrainLang(object obj)
        {
            // Powrót do ekranu zakładek
            ChangeViewModel(Vms[1]);
        }

        #endregion
    }
}
