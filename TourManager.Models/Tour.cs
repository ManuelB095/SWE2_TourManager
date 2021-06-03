using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TourManagerModels
{
        public class Tour : INotifyPropertyChanged, Item, IDataErrorInfo
        {
            private string _Name;
            private string _TourDescription;
            private string _RouteInformation;
            private double _TourDistance;

            public string Error { get; set; }

            public Tour(string name, string description, string routeInfo, double distance)
            {
                this._Name = name;
                this._TourDescription = description;
                this._RouteInformation = routeInfo;
                this._TourDistance = distance;
                this.Error = "";
            }

        public string this[string propertyName]
        {
            get { return GetErrorForProperty(propertyName); }
        }

        public string Name
            {
                get { return this._Name; }
                set
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
            public string TourDescription
            {
                get { return this._TourDescription; }
                set
                {
                    _TourDescription = value;
                    OnPropertyChanged("TourDescription");
                }
            }
            public string RouteInformation
            {
                get { return this._RouteInformation; }
                set
                {
                    _RouteInformation = value;
                    OnPropertyChanged("RouteInformation");
                }
            }
            public double TourDistance
            {
                get { return this._TourDistance; }
                set
                {
                    _TourDistance = value;
                    OnPropertyChanged("TourDistance");
                }
            }

        private string GetErrorForProperty(string propertyName)
        {
            Error = "";
            String NameRegEx = @"[A-z]+";

            switch (propertyName)
            {
                case "Name":
                    Match m = Regex.Match(_Name, NameRegEx);
                    if (_Name.Length > 4)
                    {
                        Error = "Tour Name cannot be longer than 40 chars!";
                        return Error;
                    }
                    else if(!m.Success)
                    {
                        Error = "Tour Name can only consist of characters A-Z or a-z!";
                        return Error;
                    }
                    break;
                case "TourDescription":
                    if (_TourDescription.Length >= 250)
                    {
                        Error = "Tour Description cannot be longer than 250 chars!";
                        return Error;
                    }
                    break;
            }

            return string.Empty;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
