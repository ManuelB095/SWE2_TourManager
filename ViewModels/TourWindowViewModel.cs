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
using TourManager.Commands;
using TourManager.Models;

namespace TourManager.ViewModels
{    
    public class TourWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Tour> _Tours = new ObservableCollection<Tour>(); // Use ObservableCollection
        private Tour currentTour;
        private Tour tourSelected;
        public RelayCommand UpdateRelay { get; }

        public Tour CurrentTour
        {
            get { return this.currentTour; }
            set
            {
                this.currentTour = value;
                OnPropertyChanged(nameof(currentTour));
            }
        }

        public Tour TourSelected
        {
            get { return this.tourSelected; }
            set
            {
                this.tourSelected = value;
                this.CurrentTour = value;
                OnPropertyChanged(nameof(tourSelected));
            }
        }

        public TourWindowViewModel()
        {
            UpdateRelay = new RelayCommand((o) =>
            {
                OnPropertyChanged(nameof(currentTour.Name));
                OnPropertyChanged(nameof(currentTour.RouteInformation));
                OnPropertyChanged(nameof(currentTour.TourDescription));
                OnPropertyChanged(nameof(currentTour.TourDistance));
                MessageBox.Show(currentTour.Name);
                MessageBox.Show(tourSelected.Name);

            });

            this.currentTour = new Tour("", "", "", 0);

            this._Tours.Add(new Tour("Gemäßigte Stadtroute",
                                    "Eine aussichtsreiche Zugfahrt, bei der selbst Einsteiger und Babys mitmachen können.",
                                    "Von Wien nach Linz in 1:15h", 3.5));
            this._Tours.Add(new Tour("Bergweg nach Mordor",
                                    "Man geht nicht einfach nach Mordor heißt es im Volksmund. " +
                                    "Doch das hielt die Wunderpolinger Wanderspatzen nicht auf " +
                                    "und sie haben trotzdem eine Route angelegt. " +
                                    "Treten auch sie in die Fußstapfen von Frodo und Co mit " +
                                    "dieser wunderschönen Bergtour",
                                    "Von Wien nach Mordor in 96:45", 365));
            this._Tours.Add(new Tour("Kurz zum Spar",
                                    "Haben sie es satt, dass wiedermal kein Essen im Kühlschrank ist? Das muss nicht sein! Mit unserer Kurz-Zum-Spar Wanderroute kommen sie " +
                                    "direkt an jedermanns liebstem Lebensmittelfachhändler vorbei!",
                                    "Zum Spar in weiß ich nicht, 5-10 Minuten?", 1));

        }

        public ObservableCollection<Tour> Tours
        {
            get { return _Tours; }
            set
            {
                _Tours = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}

