using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Tour> _Tours = new ObservableCollection<Tour>();
        private ObservableCollection<Log> _Logs = new ObservableCollection<Log>();
        private Tour tourSelected;
        private ImageSource routeImage;

        public ICommand NavigateEditToursCommand { get; }
        public ICommand NavigateEditLogsCommand { get; }
        public ITourItemFactory tourItemFactory;

        public ImageSource RouteImage
        {
            get { return this.routeImage; }
            set
            {
                this.routeImage = value;
                OnPropertyChanged(nameof(RouteImage));
            }
        }

        public Tour TourSelected
        {
            get { return this.tourSelected; }
            set
            {
                this.tourSelected = value;
                SetNewBitmapImage(this.tourSelected.RouteInformation);
                GetLogsFromName(tourSelected.Name);
                OnPropertyChanged(nameof(tourSelected));
            }
        }

        public HomeViewModel(NavigationStore navStore, ITourItemFactory factInstance)
        {
            this.tourItemFactory = factInstance;
            NavigateEditToursCommand = new NavigateCommand<EditToursViewModel>(navStore, () => new EditToursViewModel(navStore, tourItemFactory));
            NavigateEditLogsCommand = new NavigateCommand<EditLogsViewModel>(navStore, () => new EditLogsViewModel(navStore, tourItemFactory));

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

        public ObservableCollection<Log> Logs
        {
            get { return _Logs; }
            set
            {
                _Logs = value;
            }
        }

        private void FillTourItems()
        {
            foreach (Tour t in this.tourItemFactory.GetTours())
            {
                this.Tours.Add(t);
            }
        }

        public void GetLogsFromName(string tourName)
        {
            if(tourName != "")
            {
                this.Logs.Clear();
                foreach (Log l in this.tourItemFactory.GetLogs(tourName))
                {
                    this.Logs.Add(l);
                }
            }            
        }

        public void RefillData(string tourName)
        {
            var match = this.Tours.FirstOrDefault(toursToCheck => toursToCheck.Name.Contains(tourName));
            if (match != null)
                this.TourSelected = match; //Change Here triggers change in Logs in the Setter too!
        }

        public void SetNewBitmapImage(string routeInformation)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(routeInformation);
                image.DecodePixelHeight = 240;
                image.DecodePixelWidth = 310;
                image.EndInit();
                
                this.RouteImage = image;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}
