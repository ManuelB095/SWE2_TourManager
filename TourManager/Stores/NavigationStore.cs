using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManager.ViewModels;

namespace TourManager.Stores
{
    public class NavigationStore
    {
        public event Action currentViewModelChanged;
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            currentViewModelChanged?.Invoke();
        }

    }
}
