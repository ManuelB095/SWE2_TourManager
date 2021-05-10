using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public class CreateLogsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ICommand NavigateHomeCommand { get; set; }
        public ITourItemFactory tourItemFactory { get; }
        public event EventHandler<Log> LogCreated;
        public RelayCommand CreateLogCommand { get; }

        private string _tourName;
        private string _logDate;
        private string _logDistance;
        private string _logTotalTime;
        private string _logRating;
        private string _vehicle;
        private string _report;
        private bool _steepsections;
        private bool _scenic;
        private string _difficultyLevel;

        public string tourDistance { get; set; }

        public CreateLogsViewModel(NavigationStore navStore)
        {
            tourItemFactory = TourItemFactory.GetInstance();
            CreateLogCommand = new RelayCommand(CreateLog);
            this.TourName = "(No Tour Selected)";
            EmptyFields(); //Sets all fields to being empty
        }

        private void CreateLog(object commandParameter)
        {
            // TODO: Make Better Checking Function
            try
            {
                DateTime ParsedLogDate = DateTime.Parse(LogDate);
                double LogDistanceAsDouble = Convert.ToDouble(LogDistance);
                double LogRatingAsDouble = Convert.ToDouble(LogRating);
                int DifficultyLvlAsInt = Convert.ToInt32(DifficultyLevel);
                TimeSpan LogTotalTimeTimeSpan = TimeSpan.Parse(LogTotalTime);
                tourItemFactory.AddLog(TourName, ParsedLogDate, LogDistanceAsDouble, LogTotalTimeTimeSpan, LogRatingAsDouble, Vehicle, Report, SteepSections, Scenic, DifficultyLvlAsInt);

            }
            catch (InvalidCastException e)
            {
                MessageBox.Show(e.Message);
            }

            MessageBox.Show("Successfully created Log!");
            EmptyFields(); // Empties Fields so a new Log can be created            
        }

        public string TourName
        {
            get { return this._tourName; }
            set
            {
                this._tourName = value;
                OnPropertyChanged(nameof(TourName));
            }
        }
        public string LogDate
        {
            get { return this._logDate; }
            set
            {
                this._logDate = value;
                OnPropertyChanged(nameof(LogDate));
            }
        }
        public string LogDistance
        {
            get { return this._logDistance; }
            set
            {
                this._logDistance = value;
                OnPropertyChanged(nameof(LogDistance));
            }
        }
        public string LogTotalTime
        {
            get { return this._logTotalTime; }
            set
            {
                this._logTotalTime = value;
                OnPropertyChanged(nameof(LogTotalTime));
            }
        }

        public string LogRating
        {
            get { return this._logRating; }
            set
            {
                this._logRating = value;
                OnPropertyChanged(nameof(LogRating));
            }
        }
        public string Vehicle
        {
            get { return this._vehicle; }
            set
            {
                this._vehicle = value;
                OnPropertyChanged(nameof(Vehicle));
            }
        }
        public string Report
        {
            get { return this._report; }
            set
            {
                this._report = value;
                OnPropertyChanged(nameof(Report));
            }
        }
        public bool SteepSections
        {
            get { return this._steepsections; }
            set
            {
                this._steepsections = value;
                OnPropertyChanged(nameof(SteepSections));
            }
        }
        public bool Scenic
        {
            get { return this._scenic; }
            set
            {
                this._scenic = value;
                OnPropertyChanged(nameof(Scenic));
            }
        }
        public string DifficultyLevel
        {
            get { return this._difficultyLevel; }
            set
            {
                this._difficultyLevel = value;
                OnPropertyChanged(nameof(DifficultyLevel));
            }
        }

        public void UpdateTourName(string tourName)
        {
            this.TourName = tourName;
        }

        private void EmptyFields()
        {
            this.LogDate = DateTime.Now.ToString();
            this.LogDistance = "";
            this.LogTotalTime = "";
            this.LogRating = "";
            this.Vehicle = "";
            this.Report = "";
            this.SteepSections = false;
            this.Scenic = false;
            this.DifficultyLevel = "1";
        }
    }
}

