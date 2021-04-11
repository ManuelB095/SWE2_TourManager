using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TourManagerModels
{
        public class Tour : INotifyPropertyChanged, Item
        {
            private string _Name;
            private string _TourDescription;
            private string _RouteInformation;
            private double _TourDistance;

            public Tour(string name, string description, string routeInfo, double distance)
            {
                this._Name = name;
                this._TourDescription = description;
                this._RouteInformation = routeInfo;
                this._TourDistance = distance;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
