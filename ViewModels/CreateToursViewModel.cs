using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourManager.Commands;
using TourManager.Stores;

namespace TourManager.ViewModels
{
    public class CreateToursViewModel : BaseViewModel
    {
        public ICommand NavigateHomeCommand { get; }

        public CreateToursViewModel(NavigationStore navStore)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navStore, () => new HomeViewModel(navStore));
        }
    }
}
