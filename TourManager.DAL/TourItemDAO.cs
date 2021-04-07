using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;

namespace TourManager.DAL
{
    public class TourItemDAO // Data Access Object
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

        // Why do we do this?
        // Adding a DAO makes it possible to get data from different sources, which 
        // only need to be specified here.
        // Since all Sources follow the IDataAccess-Interface, every Source should
        // work as intended, whether it be DB or a File


    }
}
