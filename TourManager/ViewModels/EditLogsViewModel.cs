using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourManager.BusinessLayer;
using TourManager.Commands;
using TourManager.Stores;
using TourManagerModels;

namespace TourManager.ViewModels
{
    public class EditLogsViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        private ObservableCollection<Log> _Logs = new ObservableCollection<Log>();
        public ICommand NavigateHomeCommand { get; set; }
        public ITourItemFactory tourItemFactory { get; }
        public event EventHandler<bool> LogEdited;
        public RelayCommand UpdateLogCommand { get; }
        public RelayCommand DeleteLogCommand { get; }


        private string _tourName;
        private Log selectedLog;

        private string _logDate;
        private string _logDistance;
        private string _logTotalTime;
        private string _logRating;
        private string _vehicle;
        private string _report;
        private bool _steepsections;
        private bool _scenic;
        private string _difficultyLevel;

        private string _error;

        public string tourDistance { get; set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public EditLogsViewModel(NavigationStore navStore, ITourItemFactory factInstance)
        {
            tourItemFactory = factInstance;
            UpdateLogCommand = new RelayCommand(UpdateLog);
            this.TourName = "";
            this.Error = "";
            DeleteLogCommand = new RelayCommand(DeleteSelectedLog);
            GetLogsFromName(this.TourName);
        }

        private void UpdateLog(object commandParameter)
        {
            if(HasEmptyInputs())
            {
                Error = "Not all fields have a value. Please fill in the remaining fields";
            }

            if(Error == "")
            {
                double upDistance = Convert.ToDouble(LogDistance);
                TimeSpan upTotalTime = TimeSpan.FromMinutes(Int32.Parse(LogTotalTime));
                double upRating = Convert.ToDouble(LogRating);
                int upDifficultyLevel = Convert.ToInt32(DifficultyLevel);

                tourItemFactory.UpdateLog(TourName, SelectedLog.Date, upDistance, upTotalTime, upRating, Vehicle, Report, SteepSections, Scenic, upDifficultyLevel);
                MessageBox.Show("Successfully edited Log !");
                OnLogEdited(true);
            }            
        }

        public ObservableCollection<Log> Logs
        {
            get { return this._Logs; }
            set
            {
                this._Logs = value;
                OnPropertyChanged(nameof(Logs));
            }
        }

        public Log SelectedLog
        {
            get { return selectedLog; }
            set
            {
                selectedLog = value;
                if (selectedLog != null) { FitInputFields(); }
                OnPropertyChanged(nameof(SelectedLog));
            }
        }

        public string TourName
        {
            get { return this._tourName; }
            set
            {
                this._tourName = value;
                GetLogsFromName(TourName);
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

        private void GetLogsFromName(string tourName)
        {
            if (tourName != "")
            {
                this.Logs.Clear();
                foreach (Log l in this.tourItemFactory.GetLogs(tourName))
                {
                    this.Logs.Add(l);
                }
            }
        }

        public void UpdateTourName(string tourName)
        {
            this.TourName = tourName;            
        }

        public void DeleteSelectedLog(object parameter)
        {
            if(SelectedLog != null)
            {
                tourItemFactory.DeleteLog(TourName, SelectedLog.Date);
                OnLogEdited(true);
            }            
        }

        public void FitInputFields()
        {
            this.LogDate = SelectedLog.Date.ToString();
            this.LogDistance = Convert.ToString(SelectedLog.Distance);
            this.LogTotalTime = Convert.ToString(SelectedLog.TotalTime.TotalMinutes);
            this.LogRating = Convert.ToString(SelectedLog.Rating);
            this.Vehicle = SelectedLog.Vehicle;
            this.Report = SelectedLog.Report;
            this.DifficultyLevel = Convert.ToString(SelectedLog.DifficultyLevel);
            this.Scenic = SelectedLog.IsScenic;
            this.SteepSections = SelectedLog.SteepSections;
        }

        public bool HasEmptyInputs()
        {
            if (LogDate == "" || LogDistance == "" || LogTotalTime == "" || LogRating == "" || Vehicle == "" || Report == "" || DifficultyLevel == "")
            {
                return true;
            }
            else if (LogDate == null || LogDistance == null || LogTotalTime == null || LogRating == null || Vehicle == null || Report == null || DifficultyLevel == null)
            {
                return true;
            }
            else if (TourName == null || TourName == "")
                { return true; }
            return false;
        }

        private void OnLogEdited(bool madeChanges)
        {
            GetLogsFromName(this.TourName);
            LogEdited?.Invoke(this, madeChanges);
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
        public string GetErrorForProperty(string propertyName)
        {

            if (this.TourName != "" && this.TourName != null && this.SelectedLog != null)
            {
                double outValue = -1;
                int outRating = -1;
                string distanceString = "";
                int outTotalTime = -1;

                String Numerical = @"^[\d]+$";

                if (LogDistance != null && LogRating != null)
                {
                    distanceString = LogDistance.Replace(',', '.'); //Dynamically change/correct erroneous separators
                    var format = new NumberFormatInfo();
                    format.NegativeSign = "-";
                    double.TryParse(distanceString, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, format, out outValue);
                    Int32.TryParse(LogRating, out outRating);
                    Int32.TryParse(LogTotalTime, out outTotalTime);
                }

                switch (propertyName)
                {
                    case "LogDistance":
                        if (distanceString == "")
                        {
                            Error = "";
                            return Error;
                        }

                        if (outValue != 0)
                        {
                            if (outValue < 0.0)
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
                    case "LogRating":
                        if (LogRating == "")
                        {
                            Error = "";
                            return Error;
                        }

                        if (outRating != 0)
                        {
                            if (outRating > 10)
                            {
                                Error = "Rating cannot be higher than 10";
                                return Error;
                            }
                            else if (outRating < 0)
                            {
                                Error = "Rating must be a positive integer value";
                                return Error;
                            }
                        }
                        else
                        {
                            Error = "Rating has to be a number (no decimals) between 1 and 10";
                            return Error;
                        }
                        break;
                    case "LogTotalTime":
                        Match m_num = Regex.Match(_logTotalTime, Numerical);
                        if (LogTotalTime == "")
                        {
                            Error = "";
                            return Error;
                        }
                        if (!m_num.Success)
                        {
                            Error = "Total Time can only consist of nonnegative whole numbers!";
                            return Error;
                        }
                        break;
                }
            }
            return string.Empty;
        }
    }
}

