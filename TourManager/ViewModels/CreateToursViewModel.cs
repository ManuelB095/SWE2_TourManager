using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TourManager.BusinessLayer;
using TourManager.Commands;
using TourManager.Stores;
using TourManagerModels;

namespace TourManager.ViewModels
{
    public class CreateToursViewModel : BaseViewModel, IDataErrorInfo
    {
        public ICommand NavigateHomeCommand { get; set; }
        public ITourItemFactory tourItemFactory { get; }
        public event EventHandler<bool> TourCreated;
        public RelayCommand CreateTourCommand { get; }

        private string _tourName;
        private string _tourDescription;
        private string _from;
        private string _to;

        private string _error;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            // TO DO: Handle MapAPI Connection from within Business Layer, so it is more consistent
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

        public string Error
        {
            get { return _error; }
            set
            {
                this._error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName);
            }
        }
        private string GetErrorForProperty(string propertyName)
        {
            Error = "";
            String Alphabetical = @"[A-z]+";

            switch (propertyName)
            {
                case "tourName":
                    Match m = Regex.Match(_tourName, Alphabetical);
                    if (_tourName.Length > 40)
                    {
                        Error = "Tour Name cannot be longer than 40 chars!";
                        log.Debug("Tour Name is longer than allowed at the moment.");
                        return Error;
                    }
                    else if(_tourName.Length < 1)
                    {
                        Error = "Field Tour Name is empty!";
                        return Error;
                    }
                    else if (!m.Success)
                    {
                        Error = "Tour Name can only consist of characters A-Z or a-z!";
                        log.Debug("Tour Name has unallowed input at the moment.");
                        return Error;
                    }
                    break;
                case "TourDescription":
                    if (_tourDescription.Length >= 250)
                    {
                        Error = "Tour Description cannot be longer than 250 chars!";
                        log.Debug("Tour Description is longer than allowed at the moment.");
                        return Error;
                    }
                    break;
                case "From":
                    Match m2 = Regex.Match(_from, Alphabetical);
                    if (!m2.Success)
                    {
                        Error = "From Field did not match Regex.";
                        return Error;
                    }
                    break;
                case "To":
                    Match m3 = Regex.Match(_to, Alphabetical);
                    if (!m3.Success)
                    {
                        Error = "To Field did not match Regex.";
                        return Error;
                    }
                    break;
            }
            return string.Empty;

        }


    }
}
