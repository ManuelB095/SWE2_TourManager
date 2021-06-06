using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TourManagerModels
{
    public class Log : INotifyPropertyChanged, Item
    {
        private string _tourname; // Foreign Key that identifies which tour the log is for
        private DateTime _date; // ex: '2020-09-02 03:00'
        private string _report;
        private double _distance;
        private TimeSpan _totalTime; // ex: '00:20:00' equals 20 Minutes
        private double _rating; // Rating between 0.0 and 10.0
        //5 more...
        private string _vehicle; //can be null, or certain bike type
        private double _velocity; // calculated via speed/totalTime; >= 0.0
        private bool _steepSections; // Yes/NO
        private bool _scenic;  // Yes/NO
        private int _difficultyLevel; // Rating between 1-5

        public Log(string tourname, DateTime date, string report, double distance, TimeSpan totalTime, double rating, string vehicle, double velocity, bool steepSections, bool scenic, int diffLevel)
        {
            this.Tourname = tourname;
            this.Date = date;
            this.Report = report;
            this.Distance = distance;
            this.TotalTime = totalTime;
            this.Rating = rating;
            this.Vehicle = vehicle;
            this.Velocity = velocity;
            this.SteepSections = steepSections;
            this.IsScenic = scenic;
            this.DifficultyLevel = diffLevel;
        }

        public Log()
        {

        }

        public string Tourname
        {
            get { return _tourname; }
            set
            {
                _tourname = value;
                OnPropertyChanged(nameof(Tourname));
            }
        }


        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public string Report
        {
            get { return _report; }
            set
            {
                _report = value;
                OnPropertyChanged(nameof(Report));
            }
        }
        public double Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged(nameof(Distance));
            }
        }
        public TimeSpan TotalTime
        {
            get { return _totalTime; }
            set
            {
                _totalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }
        public double Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        public string Vehicle
        {
            get { return _vehicle; }
            set
            {
                _vehicle = value;
                OnPropertyChanged(nameof(Vehicle));
            }
        }

        public int DifficultyLevel
        {
            get { return _difficultyLevel; }
            set
            {
                _difficultyLevel = value;
                OnPropertyChanged(nameof(DifficultyLevel));
            }
        }
        public bool SteepSections
        {
            get { return _steepSections; }
            set
            {
                _steepSections = value;
                OnPropertyChanged(nameof(SteepSections));
            }
        }
        public bool IsScenic
        {
            get { return _scenic; }
            set
            {
                _scenic = value;
                OnPropertyChanged(nameof(IsScenic));
            }
        }
        public double Velocity
        {
            get { return _velocity; }
            set
            {
                _velocity = value;
                OnPropertyChanged(nameof(Velocity));
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
