using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourManager.BusinessLayer;
using TourManager.Commands;
using TourManagerModels;
using TourManager.Stores;

namespace TourManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {        
        private readonly NavigationStore navigationStore;
        private ITourItemFactory tourItemFactory;
        public ObservableCollection<Tour> TourItems { get; }

        public BaseViewModel CurrentViewModel => navigationStore.CurrentViewModel;
        public ICommand NavigateEditToursCommand { get; }
        public ICommand NavigateCreateToursCommand { get; }


        public MainViewModel(NavigationStore navStore)
        {

            navigationStore = navStore;
            navigationStore.currentViewModelChanged += OnCurrentViewModelChanged;
            NavigateEditToursCommand = new NavigateCommand<EditToursViewModel>(navigationStore, () => new EditToursViewModel(navigationStore));
            NavigateCreateToursCommand = new NavigateCommand<CreateToursViewModel>(navigationStore, () => new CreateToursViewModel(navigationStore));

            this.tourItemFactory = TourItemFactory.GetInstance();
            this.TourItems = new ObservableCollection<Tour>();
            FillTourItems();
        }

        private void FillTourItems()
        {
            foreach (Tour t in this.tourItemFactory.GetTours())
            {
                this.TourItems.Add(t);
            }
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
