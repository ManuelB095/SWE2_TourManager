using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourManager.BusinessLayer
{
    public static class ReportGenerator
    {
        private static IReportGenerator _instance;

        public static IReportGenerator GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ReportGeneratorImpl();
            }
            return _instance;
        }
    }
}
