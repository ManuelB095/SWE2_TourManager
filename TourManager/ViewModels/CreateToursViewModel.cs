using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourManager.BusinessLayer;
using TourManager.Commands;
using TourManager.Stores;
using TourManagerModels;

namespace TourManager.ViewModels
{
    public class CreateToursViewModel : BaseViewModel
    {
        public ICommand NavigateHomeCommand { get; set; }
        public ITourItemFactory tourItemFactory { get; }
        public event EventHandler<bool> TourCreated;
        public RelayCommand CreateTourCommand { get; }

        private string _tourName;
        private string _tourDescription;
        private string _from;
        private string _to;

        public CreateToursViewModel(NavigationStore navStore)
        {
            tourItemFactory = TourItemFactory.GetInstance();
            CreateTourCommand = new RelayCommand(CreateTour);
            this.tourName = "TourName";
            this.tourDescription = "Description";
            this.From = "From";
            this.To = "To";
        }

        private void CreateTour(object commandParameter)
        {           
            MapAPIConnection mConn = new MapAPIConnection();

            mConn.HandleMapQuestRequest(this.From, this.To);
            string filePath = mConn.ResultingFilePath;
            double distance = mConn.distance;
            tourItemFactory.AddTour(tourName, tourDescription, filePath, distance);
            System.Windows.MessageBox.Show("Successfully created a new Tour!", "Created Tour", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            OnTourCreated(true);
        }

        public string tourName
        {
            get { return this._tourName; }
            set 
            {
                this._tourName = value;
                OnPropertyChanged(nameof(tourName));
            }
        }
        public string tourDescription
        {
            get { return this._tourDescription; }
            set
            {
                this._tourDescription = value;
                OnPropertyChanged(nameof(tourDescription));
            }
        }
        public string From
        {
            get { return this._from; }
            set
            {
                this._from = value;
                OnPropertyChanged(nameof(From));
            }
        }
        public string To
        {
            get { return this._to; }
            set
            {
                this._to = value;
                OnPropertyChanged(nameof(To));
            }
        }

        private void OnTourCreated(bool created)
        {
            TourCreated?.Invoke(this, created);
        }
    }
}
