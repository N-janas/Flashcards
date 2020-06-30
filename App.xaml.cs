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
            MainWindow mainWindow = new MainWindow();
            MainViewModel context = new MainViewModel();
            mainWindow.DataContext = context;
            Application.Current.Resources["sharedViewModel"] = context;

            mainWindow.Show();
        }
    }
}
