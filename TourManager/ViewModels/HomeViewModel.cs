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
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Tour> _Tours = new ObservableCollection<Tour>();
        private ObservableCollection<Log> _Logs = new ObservableCollection<Log>();
        private Tour tourSelected;
        public ICommand NavigateEditToursCommand { get; }
        private ITourItemFactory tourItemFactory;

        public Tour TourSelected
        {
            get { return this.tourSelected; }
            set
            {
                this.tourSelected = value;
                GetLogsFromName(tourSelected.Name);
                OnPropertyChanged(nameof(tourSelected));
            }
        }

        public HomeViewModel(NavigationStore navStore)
        {           

            NavigateEditToursCommand = new NavigateCommand<EditToursViewModel>(navStore, () => new EditToursViewModel(navStore));
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

        private void GetLogsFromName(string tourName)
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

    }
}
