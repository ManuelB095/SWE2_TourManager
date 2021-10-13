using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TourManager.BusinessLayer;
using TourManager.Commands;
using TourManager.Stores;
using TourManagerModels;

namespace TourManager.ViewModels
{    
    public class EditToursViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        private ObservableCollection<Tour> _Tours;
        private Tour tourSelected;
        private ImageSource routeImage;

        private string _tourDescription;
        private string _tourDistance;

        private string _error;

        public string currentTourDistance;
        public RelayCommand UpdateRelay { get; }
        public ICommand NavigateHomeCommand { get; set; }
        public ITourItemFactory tourItemFactory { get; set; }

        public event EventHandler<bool> TourEdited;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public Tour TourSelected
        {
            get { return this.tourSelected; }
            set
            {
                this.tourSelected = value;
                if (tourSelected != null) { FitInputFields(); }
                SetNewBitmapImage(this.tourSelected.RouteInformation);
                OnPropertyChanged(nameof(tourSelected));
            }
        }
        public string tourDistance
        {
            get { return this._tourDistance; }
            set
            {
                this._tourDistance = value;
                OnPropertyChanged(nameof(tourDistance));
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

        public ImageSource RouteImage
        {
            get {return routeImage; }
            set
            {
                routeImage = value;
                OnPropertyChanged(nameof(RouteImage));
            }
        }

        public EditToursViewModel(NavigationStore navStore, ITourItemFactory factInstance)
        {
            UpdateRelay = new RelayCommand(UpdateTour);
            this.tourItemFactory = factInstance;
            this.Tours = new ObservableCollection<Tour>();
            this.Error = "";
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
            if (HasEmptyInputs())
            {
                Error = "Not all fields have a value. Please fill in the remaining fields";
            }

            if(Error == "")
            {
                tourItemFactory.UpdateTour(TourSelected.Name, tourDescription, TourSelected.RouteInformation, Convert.ToDouble(tourDistance));
                OnTourEdited(true);
                MessageBox.Show("Successfully updated Tour '" + TourSelected.Name + "' .", "Tour Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                log.Info("Updated Tour " + TourSelected.Name);
            }            
        }

        private void OnTourEdited(bool madeChanges)
        {
            TourEdited?.Invoke(this, madeChanges);
        }
        public void SetNewBitmapImage(string routeInformation)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(routeInformation);
            image.EndInit();
            this.RouteImage = image;
        }
        public void FitInputFields()
        {
            this.tourDescription = TourSelected.TourDescription;
            this.tourDistance = Convert.ToString(TourSelected.TourDistance);
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
        public bool HasEmptyInputs()
        {
            if (tourDescription == "" || tourDistance == "")
            {
                return true;
            }
            else if (tourDescription == null || tourDistance == null)
            {
                return true;
            }
            else if (TourSelected.Name == "" || TourSelected.Name == null)
            { return true; }
            return false;
        }

        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName);
            }
        }
        public string GetErrorForProperty(string propertyName)
        {
            if(this.TourSelected != null)
            {
                string distanceString = "";
                double outDistance = -1;

                if (tourDistance != null)
                {
                    distanceString = tourDistance.Replace(',', '.'); //Dynamically change/correct erroneous separators
                    var format = new NumberFormatInfo();
                    format.NegativeSign = "-";
                    double.TryParse(distanceString, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, format, out outDistance);
                }

                String Alphabetical = @"[A-Za-z]+";
                String Numerical = @"[\d]";

                switch (propertyName)
                {
                    case "tourDescription":
                        if (_tourDescription.Length >= 250)
                        {
                            Error = "Tour Description cannot be longer than 250 chars!";
                            log.Debug("Tour Description is longer than allowed at the moment.");
                            return Error;
                        }
                        break;
                    case "tourDistance":
                        if (distanceString == "")
                        {
                            Error = "";
                            return Error;
                        }

                        if (outDistance != 0)
                        {
                            if (outDistance < 0.0)
                            {
                                Error = "Log Distance can not be smaller than Zero!";
                                return Error;
                            }
                        }
                        else
                        {
                            Error = "Log Distance can only use integer or decimal numbers! Use only numbers from 0-9!";
                            return Error;
                        }
                        break;
                }                
            }
            return string.Empty;
        }
    }
}

