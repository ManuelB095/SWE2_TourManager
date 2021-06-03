using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;

namespace TourManager.BusinessLayer
{
    public interface IReportGenerator
    {
        public void GenerateTourReport(Tour t);
    }
}
