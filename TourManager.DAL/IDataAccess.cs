using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;

namespace TourManager.DAL
{
    public interface IDataAccess // TO DO: Change to more generic type, for example "Item"
    {
        public List<Tour> GetItems();
        public Tour GetTourByName(string _tourName);
    }
}
