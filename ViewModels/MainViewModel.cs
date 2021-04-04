using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourManager.ViewModels
{
    public class MainViewModel
    {
        public BaseViewModel CurrentViewModel { get; }

        public MainViewModel()
        {
            CurrentViewModel = new HomeViewModel();
        }
    }
}
