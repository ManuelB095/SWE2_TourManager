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
        public string GetPath();
        public bool ImgExists { get; }
        public void ChangeOutputPath(string newPath);
        public void GenerateTourReport(Tour t);
    }
}
