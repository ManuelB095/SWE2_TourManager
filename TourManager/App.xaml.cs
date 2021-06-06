using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourManager.BusinessLayer;
using TourManager.Stores;
using TourManager.ViewModels;

namespace TourManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();
            ITourItemFactory factInstance = TourItemFactory.GetInstance();
            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore, factInstance);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore, factInstance)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
