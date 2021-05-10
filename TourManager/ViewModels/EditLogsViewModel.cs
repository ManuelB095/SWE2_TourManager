using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class EditLogsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<Log> _Logs = new ObservableCollection<Log>();
        public ICommand NavigateHomeCommand { get; set; }
        public ITourItemFactory tourItemFactory { get; }
        public event EventHandler<bool> LogEdited;
        public RelayCommand UpdateLogCommand { get; }

        private string _tourName;
        private Log selectedLog;

        public string tourDistance { get; set; }

        public EditLogsViewModel(NavigationStore navStore)
        {
            tourItemFactory = TourItemFactory.GetInstance();
            UpdateLogCommand = new RelayCommand(UpdateLog);
            this.TourName = "";
            GetLogsFromName(this.TourName);
        }

        private void UpdateLog(object commandParameter)
        {                          
            tourItemFactory.UpdateLog(TourName, SelectedLog.Date, SelectedLog.Distance, SelectedLog.TotalTime, SelectedLog.Rating, SelectedLog.Vehicle, SelectedLog.Report, SelectedLog.SteepSections, SelectedLog.IsScenic, SelectedLog.DifficultyLevel);
            MessageBox.Show("Successfully edited Log !");
            OnLogEdited(true);
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

        //public void RefillData(DateTime selectedLogDate)
        //{
        //    var match = this.Logs.FirstOrDefault(logsToCheck => logsToCheck.Date.Equals(selectedLogDate));
        //    if (match != null)
        //        this.SelectedLog = match;
        //}

        private void OnLogEdited(bool madeChanges)
        {
            GetLogsFromName(this.TourName);
            LogEdited?.Invoke(this, madeChanges);
        }
    }
}

