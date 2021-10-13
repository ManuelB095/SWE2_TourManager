using System;
using System.Collections.Generic;
using System.Linq;
using TourManager.DAL;
using TourManagerModels;

namespace TourManager.BusinessLayer
{
    internal class TourItemFactoryImpl : ITourItemFactory
    {
        private ITourItemDAO databaseDAO = new TourItemDAO();
        private FileHandler fileHandler = new FileHandler();
        public List<Tour> TourItems { get; } = new List<Tour>();

        public string CurrentlySelectedTourName { get; set; }

        public IEnumerable<Tour> GetTours()
        {
            return databaseDAO.GetItems();
        }

        public IEnumerable<Log> GetLogs(string tourName)
        {
            return databaseDAO.GetLogs(tourName);
        }


        public IEnumerable<Tour> Search(string itemName, bool caseSensitive = false)
        {
            IEnumerable<Tour> allTourItems = GetTours();
            if (caseSensitive)
            {
                return allTourItems.Where(x => x.Name.Contains(itemName));
            }
            return allTourItems.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public void AddTour(string name, string description, string routeInfo, double distance)
        {
            databaseDAO.AddTour(name, description, routeInfo, distance);            
        }

       public void AddLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel)
        {
           databaseDAO.AddLog(tourName, logDate, logDistance, logTotalTime, LogRating, vehicle, report, steepSections, scenic, difficultyLevel);

        }

        public void UpdateTour(string tourName, string description, string routeInfo, double distance)
        {
            databaseDAO.UpdateTour(tourName, description, routeInfo, distance);
        }
        public void UpdateLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel)
        {
            databaseDAO.UpdateLog(tourName, logDate, logDistance, logTotalTime, LogRating, vehicle, report, steepSections, scenic, difficultyLevel);
        }

        public void DeleteTour(string tourName, string routeInformation)
        {
            fileHandler.DeleteEntryByName(routeInformation);
            databaseDAO.DeleteTour(tourName);
        }

        public void DeleteLog(string tourname, DateTime timestamp)
        {
            databaseDAO.DeleteLog(tourname, timestamp);
        }

        public void ChangeDataSource(ITourItemDAO newSource)
        {
            this.databaseDAO = newSource;
        }
    }
}