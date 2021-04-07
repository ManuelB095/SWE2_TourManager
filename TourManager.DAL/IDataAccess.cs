using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;

namespace TourManager.DAL
{
    public interface IDataAccess
    {
        public List<Tour> GetItems();
    }
}
