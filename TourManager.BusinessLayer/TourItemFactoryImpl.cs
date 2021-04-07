using System.Collections.Generic;
using System.Linq;
using TourManager.DAL;
using TourManagerModels;

namespace TourManager.BusinessLayer
{
    internal class TourItemFactoryImpl : ITourItemFactory
    {
        private TourItemDAO databaseDAO = new TourItemDAO();
        public List<Tour> TourItems { get; } = new List<Tour>();

        public IEnumerable<Tour> GetTours()
        {
            return databaseDAO.GetItems();
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
    }
}