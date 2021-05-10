using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;

namespace TourManager.BusinessLayer
{
    public interface ITourItemFactory
    {
        IEnumerable<Tour> GetTours();

        IEnumerable<Log> GetLogs(string tourName);
        IEnumerable<Tour> Search(String itemName, bool caseSensitive = false);

        public string CurrentlySelectedTourName {get; set; }

        void AddTour(string name, string description, string routeInfo, double distance);
        void AddLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel);

        void UpdateTour(string tourName, string description, string routeInfo, double distance);
        void UpdateLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel);



        void DeleteTour(string tourName); // Also deletes all Log Files!
    
    
    }
}
