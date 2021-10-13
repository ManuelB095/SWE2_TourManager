using System;
using System.Collections.Generic;
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
    public class CreateLogsViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public ICommand NavigateHomeCommand { get; set; }
        public ITourItemFactory tourItemFactory { get; }
        public event EventHandler<bool> LogCreated;
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

        private string _error;
        public string tourDistance { get; set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public CreateLogsViewModel(NavigationStore navStore, ITourItemFactory factInstance)
        {
            tourItemFactory = factInstance;
            CreateLogCommand = new RelayCommand(CreateLog);
            this.Error = "";
            this.TourName = "(No Tour Selected)";
            EmptyFields(); //Sets all fields to being empty
        }

        private void CreateLog(object commandParameter)
        {
            if (HasEmptyInputs())
            {
                Error = "Not all fields have a value. Please fill in the remaining fields";
            }
            if(Error == "")
            {
                DateTime ParsedLogDate = DateTime.Parse(LogDate);
                double LogDistanceAsDouble = Convert.ToDouble(LogDistance);
                double LogRatingAsDouble = Convert.ToDouble(LogRating);
                int DifficultyLvlAsInt = Convert.ToInt32(DifficultyLevel);
                TimeSpan LogTotalTimeTimeSpan = TimeSpan.FromMinutes(Int32.Parse(LogTotalTime));
                tourItemFactory.AddLog(TourName, ParsedLogDate, LogDistanceAsDouble, LogTotalTimeTimeSpan, LogRatingAsDouble, Vehicle, Report, SteepSections, Scenic, DifficultyLvlAsInt);
                OnLogCreated(true);
                MessageBox.Show("Successfully created Log!");
                EmptyFields(); // Empties Fields so a new Log can be created
            }
                        
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

        public void UpdateTourName(string tourName)
        {
            this.TourName = tourName;
        }

        private void OnLogCreated(bool logCreated)
        {
            LogCreated?.Invoke(this, logCreated);
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
            if (this.TourName != "")
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

