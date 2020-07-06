using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlashCards
{
    using ViewModel;
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Utworzenie okna
            MainWindow mainWindow = new MainWindow();
            // Utworzenie głównego vma i wpisanie do contextu view
            MainViewModel context = new MainViewModel();
            mainWindow.DataContext = context;
            // Nadpisanie zasobu utworzonego w App.xaml, aby móc odwoływać się w plikach .xaml
            // do utworzonego tutaj wspólnego mainVM
            Application.Current.Resources["sharedViewModel"] = context;

            mainWindow.Show();
        }
    }
}
