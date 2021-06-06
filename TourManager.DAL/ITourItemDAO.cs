using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;

namespace TourManager.DAL
{
    public interface ITourItemDAO
    {
        public List<Tour> GetItems();

        public List<Log> GetLogs(string tourName);

        public void AddTour(string name, string description, string routeInfo, double distance);

        public void AddLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel);

        public void UpdateTour(string tourName, string description, string routeInfo, double distance);
        public void UpdateLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel);

        public void DeleteTour(string tourName);
    }
}
