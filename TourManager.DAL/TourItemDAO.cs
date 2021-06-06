using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;

namespace TourManager.DAL
{
    public class TourItemDAO : ITourItemDAO // Data Access Object
    {
        // Try to work independent from dataSource.
        // Trust that IDataAcces will do what it needs to

        private IDataAccess dataSource;

        public TourItemDAO()
        {
            // Could be any dataSource really
            dataSource = new PostgresDB();
        }

        public List<Tour> GetItems()
        {
            return dataSource.GetItems();
        }

        public List<Log> GetLogs(string tourName)
        {
            return dataSource.GetLogs(tourName);
        }

        public void AddTour(string name, string description, string routeInfo, double distance)
        {
            dataSource.AddTour(name, description, routeInfo, distance);
        }

        public void AddLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel)
        {
            dataSource.AddLog(tourName, logDate, logDistance, logTotalTime, LogRating, vehicle, report, steepSections, scenic, difficultyLevel);

        }

        public void UpdateTour(string tourName, string description, string routeInfo, double distance)
        {
            dataSource.UpdateTour(tourName, description, routeInfo, distance);
        }
        public void UpdateLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel)
        {
            dataSource.UpdateLog(tourName, logDate, logDistance, logTotalTime, LogRating, vehicle, report, steepSections, scenic, difficultyLevel);

        }

        public void DeleteTour(string tourName)
        {
            dataSource.DeleteTour(tourName);
        }


        // Why do we do this?
        // Adding a DAO makes it possible to get data from different sources, which 
        // only need to be specified here.
        // Since all Sources follow the IDataAccess-Interface, every Source should
        // work as intended, whether it be DB or a File


    }
}
