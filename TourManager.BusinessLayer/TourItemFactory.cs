using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourManager.BusinessLayer
{
    public static class TourItemFactory
    {
        private static ITourItemFactory _instance;

        public static ITourItemFactory GetInstance()
        {
            if(_instance == null)
            {
                _instance = new TourItemFactoryImpl();
            }
            return _instance;
        }
    }
}
