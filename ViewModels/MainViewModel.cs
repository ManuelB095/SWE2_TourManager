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
using System.Collections;

namespace TourManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {        
        private readonly NavigationStore navigationStore;
        private ITourItemFactory tourItemFactory;

        private string searchName;
        public ObservableCollection<Tour> TourItems { get; }

        public BaseViewModel CurrentViewModel => navigationStore.CurrentViewModel;
        public ICommand NavigateEditToursCommand { get; }
        public ICommand NavigateCreateToursCommand { get; }

        public RelayCommand SearchCommand { get; }
        public RelayCommand RefreshCommand { get; }


        public MainViewModel(NavigationStore navStore)
        {

            navigationStore = navStore;
            navigationStore.currentViewModelChanged += OnCurrentViewModelChanged;
            NavigateEditToursCommand = new NavigateCommand<EditToursViewModel>(navigationStore, () => new EditToursViewModel(navigationStore));
            NavigateCreateToursCommand = new NavigateCommand<CreateToursViewModel>(navigationStore, () => new CreateToursViewModel(navigationStore));

            SearchCommand = new RelayCommand(Search);
            RefreshCommand = new RelayCommand(Refresh);

            this.tourItemFactory = TourItemFactory.GetInstance();
            this.TourItems = new ObservableCollection<Tour>();
            FillTourItems();
        }

        public string SearchName
        {
            get { return searchName; }
            set
            {
                if (searchName != value)
                {
                    searchName = value;
                    OnPropertyChanged(nameof(SearchName));
                }
            }
        }

        private void Search(object commandParameter)
        {
            IEnumerable foundItem = this.tourItemFactory.Search(SearchName);
            TourItems.Clear();
            foreach(Tour t in foundItem)
            {
                TourItems.Add(t);
            }
        }

        private void Refresh(object commandParameter)
        {
            TourItems.Clear();
            SearchName = "";
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
