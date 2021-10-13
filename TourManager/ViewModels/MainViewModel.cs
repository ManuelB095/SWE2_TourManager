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
using System.Diagnostics;
using System.Windows;

namespace TourManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {        
        private readonly NavigationStore navigationStore;
        private ITourItemFactory tourItemFactory;

        private string searchName;
        private Tour selectedItem;
        private string selectedItemName;
        
        public event EventHandler<string> selectedItemChanged;

        public event EventHandler<bool> selectedItemDelete;

        private ObservableCollection<Tour> _tourItems;
        public BaseViewModel CurrentViewModel => navigationStore.CurrentViewModel;
        public ICommand NavigateEditToursCommand { get; }
        public ICommand NavigateCreateToursCommand { get; }

        public ICommand NavigateCreateLogsCommand { get; }

        public ICommand NavigateEditLogsCommand { get; }

        public ICommand DisplayHomeCommand { get; }

        public RelayCommand SearchCommand { get; }
        public RelayCommand RefreshCommand { get; } 

        public RelayCommand DeleteTourCommand { get; }

        public RelayCommand CreateTourReportCommand { get; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ObservableCollection<Tour> TourItems
        {
            get { return this._tourItems; }
            set
            {
                this._tourItems = value;
                OnPropertyChanged(nameof(TourItems));
            }
        }

        public MainViewModel(NavigationStore navStore, ITourItemFactory factInstance)
        {
            navigationStore = navStore;
            navigationStore.currentViewModelChanged += OnCurrentViewModelChanged;

            this.tourItemFactory = factInstance;
            this.TourItems = new ObservableCollection<Tour>();
            FillTourItems();

            DisplayHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () =>
            {
                var viewModel = new HomeViewModel(navigationStore, tourItemFactory );                
                this.selectedItemChanged += (_, tourName) => { viewModel.RefillData(tourName); };
                if (SelectedItem != null)
                {
                    OnSelectedItemChanged(this.SelectedItem.Name);
                }
                return viewModel;
            });

            DisplayHomeCommand.Execute(""); // object parameter requires something to pass to it

            NavigateEditToursCommand = new NavigateCommand<EditToursViewModel>(navigationStore, () =>
            {
                var viewModel = new EditToursViewModel(navigationStore, tourItemFactory);
                
                // Add EditToursViewModel`s RefillData method to subscribers. Fire event when selectedItem gets changed!                
                this.selectedItemChanged += (_, tourName) => { viewModel.RefillData(tourName); };
                if(SelectedItem != null)
                {
                    OnSelectedItemChanged(this.SelectedItem.Name);
                }
                viewModel.NavigateHomeCommand = DisplayHomeCommand;
                viewModel.TourEdited += (_, tourName) => { this.Refresh(_); };                
                return viewModel;
            });
            NavigateCreateToursCommand = new NavigateCommand<CreateToursViewModel>(navigationStore, () => 
            {
                var viewModel = new CreateToursViewModel(navigationStore, tourItemFactory);

                //Subscribe to TourCreated Event to update automatically
                viewModel.TourCreated += (_, tourCreated) => { this.Refresh(_); };
                if (SelectedItem != null)
                {
                    OnSelectedItemChanged(this.SelectedItem.Name);
                }
                viewModel.NavigateHomeCommand = DisplayHomeCommand;
                return viewModel;
            });

            NavigateCreateLogsCommand = new NavigateCommand<CreateLogsViewModel>(navigationStore, () =>
            {
                var viewModel = new CreateLogsViewModel(navigationStore, tourItemFactory);

                // Add EventSubscriber
                this.selectedItemChanged += (_, tourName) => { viewModel.UpdateTourName(tourName); };
                if (SelectedItem != null)
                {
                    OnSelectedItemChanged(this.SelectedItem.Name);
                }
                viewModel.NavigateHomeCommand = DisplayHomeCommand;
                return viewModel;
            });

            NavigateEditLogsCommand = new NavigateCommand<EditLogsViewModel>(navigationStore, () =>
            {
                var viewModel = new EditLogsViewModel(navigationStore, tourItemFactory);
                this.selectedItemChanged += (_, tourName) => { viewModel.UpdateTourName(tourName); };
                if (SelectedItem != null)
                {
                    OnSelectedItemChanged(this.SelectedItem.Name);
                }
                viewModel.NavigateHomeCommand = DisplayHomeCommand;
                viewModel.LogEdited += (_, tourCreated) => { this.Refresh(_); };
                return viewModel;
            });

            SearchCommand = new RelayCommand(Search);
            RefreshCommand = new RelayCommand(Refresh);
            DeleteTourCommand = new RelayCommand(DeleteSelectedTour);
            CreateTourReportCommand = new RelayCommand(GenerateTourReport);            
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

        public string SelectedItemName
        {
            get { return selectedItemName; }
            set
            {
                if (selectedItemName != value)
                {
                    selectedItemName = value;
                    OnPropertyChanged(nameof(SearchName));
                }
            }
        }

        public Tour SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if(value != null)
                {
                    OnSelectedItemChanged(this.SelectedItem.Name); // Fire Event to update CurrentlySelectedTour for all ViewModels!
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
            log.Info("Refreshed!");
        }

        private void FillTourItems()
        {
            foreach (Tour t in this.tourItemFactory.GetTours())
            {
                this.TourItems.Add(t);
            }
        }

        private void DeleteSelectedTour(object parameter)
        {
            var result = MessageBox.Show("The corresponding logs and all tour Information will be deleted", "Delete Tour", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if(result == MessageBoxResult.OK)
            {
                OnSelectedItemDelete(true);
                tourItemFactory.DeleteTour(selectedItem.Name, selectedItem.RouteInformation);                
                MessageBox.Show("Successfully deleted Tour " + selectedItem.Name + " .", "Tour Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshCommand.Execute("");
            }
        }

        private void GenerateTourReport(object parameter)
        {
            if(this.SelectedItem.Name != "")
            {
                IReportGenerator generator = ReportGenerator.GetInstance();
                generator.GenerateTourReport(selectedItem);
            }            
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnSelectedItemChanged(string newSelectionName)
        {
            selectedItemChanged?.Invoke(this, newSelectionName);
        }

        private void OnSelectedItemDelete(bool deleted)
        {
            selectedItemDelete?.Invoke(this, deleted);
        }

    }
}
