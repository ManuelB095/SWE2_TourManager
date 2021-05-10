using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourManager.BusinessLayer;
using TourManager.Commands;
using TourManager.Stores;
using TourManagerModels;

namespace TourManager.ViewModels
{    
    public class EditToursViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<Tour> _Tours;
        private Tour tourSelected;
        public string currentTourDistance;
        public RelayCommand UpdateRelay { get; }
        public ICommand NavigateHomeCommand { get; set; }
        public ITourItemFactory tourItemFactory { get; set; }

        public event EventHandler<bool> TourEdited;


        public Tour TourSelected
        {
            get { return this.tourSelected; }
            set
            {
                this.tourSelected = value;
                OnPropertyChanged(nameof(tourSelected));
            }
        }

        public EditToursViewModel(NavigationStore navStore)
        {
            UpdateRelay = new RelayCommand(UpdateTour);
            this.tourItemFactory = TourItemFactory.GetInstance();
            this.Tours = new ObservableCollection<Tour>();
            FillTourItems();
        }

        public ObservableCollection<Tour> Tours
        {
            get { return _Tours; }
            set
            {
                _Tours = value;
            }
        }

        private void FillTourItems()
        {
            foreach (Tour t in this.tourItemFactory.GetTours())
            {
                this.Tours.Add(t);
            }
        }

        public void RefillData(string tourName)
        {
            var match = this.Tours.FirstOrDefault(toursToCheck => toursToCheck.Name.Contains(tourName));
            if (match != null)
                this.TourSelected = match;
        }

        private void UpdateTour(object parameter)
        {
            tourItemFactory.UpdateTour(TourSelected.Name, TourSelected.TourDescription, TourSelected.RouteInformation, TourSelected.TourDistance);
            OnTourEdited(true);
            MessageBox.Show("Successfully updated Tour '" + TourSelected.Name + "' .", "Tour Updated", MessageBoxButton.OK, MessageBoxImage.Information) ;
        }

        private void OnTourEdited(bool madeChanges)
        {
            TourEdited?.Invoke(this, madeChanges);
        }


    }
}

