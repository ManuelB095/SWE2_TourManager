using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourManager.Stores;
using TourManager.ViewModels;

namespace TourManager.Commands
{
    public class NavigateCommand<TViewModel> : ICommand
        where TViewModel:BaseViewModel
    {
        public event EventHandler CanExecuteChanged;
        private readonly NavigationStore navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public NavigateCommand(NavigationStore navStore, Func<TViewModel> createViewModel)
        {
            navigationStore = navStore;
            _createViewModel = createViewModel;
        }

        public void Execute(object parameter)
        {
            navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
