using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Properties;
using System.Windows.Media.Imaging;
using iText.IO.Image;
using TourManager.DAL;
using System.Configuration;
using System.IO;

namespace TourManager.BusinessLayer
{
    public class ReportGeneratorImpl : IReportGenerator
    {
        private string path = ConfigurationManager.AppSettings["PDF_FILE_PATH"]; // TO DO: Get path from config
        private string blankImagePath = ConfigurationManager.AppSettings["BLANK_IMG_PATH"];
        private PostgresDB Dbase = new PostgresDB();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool imgExists = false;

        public bool ImgExists
        {
            get { return imgExists; }
            set
            {
                imgExists = value;
            }
        }

        public ReportGeneratorImpl()
        {
            
        }

        public void ChangeOutputPath(string newPath)
        {
            this.path = newPath;
        }

        public string GetPath()
        {
            return this.path;
        }

        public Image PrepareImgFromTour(Tour t)
        {
            int height = 120;
            int width = 180;

            try
            {
                Image img = new Image(ImageDataFactory.Create(t.RouteInformation));
                img.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                img.SetHeight(height);
                img.SetWidth(width);
                ImgExists = true;
                return img;
            }
            catch(Exception e)
            {
                ImgExists = false;
                log.Error(e.Message);
                log.Error("Means: Image of tour" + t.Name + "not found.");                
            }
            Image blank = new Image(ImageDataFactory.Create(blankImagePath));
            blank.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            blank.SetHeight(height);
            blank.SetWidth(width);
            return blank;
        }

        public void GenerateTourReport(Tour t)
        {
            string filename = t.Name + ".pdf";
            int counter = 1;
            while(File.Exists(path+filename))
            {
                filename = t.Name + "(" + Convert.ToString(counter) + ")" + ".pdf";
                ++counter;
            }

            PdfWriter write = new PdfWriter(path+filename);
            PdfDocument doc = new PdfDocument(write);
            Document pdf = new Document(doc);

            Paragraph head = new Paragraph(t.Name).SetTextAlignment(TextAlignment.CENTER).SetFontSize(24);
            pdf.Add(head);

            LineSeparator sep = new LineSeparator(new SolidLine()).SetMarginTop(10).SetMarginBottom(10);
            LineSeparator dottSep = new LineSeparator(new DottedLine()).SetMarginTop(10).SetMarginBottom(10);

            Image img = PrepareImgFromTour(t);

            pdf.Add(img);
            pdf.Add(sep);

            pdf.Add(new Paragraph($"Distance: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{t.TourDistance}"));
            pdf.Add(new Paragraph($"Description: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{t.TourDescription}"));            // pdf.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            pdf.Add(sep);
            pdf.Add(new Paragraph("Tour Logs: ").SetFontSize(18));

            List<Log> tourLogs = Dbase.GetLogs(t.Name);

            bool first = true;

            foreach(var log in tourLogs)
            {
                if (first == false)
                    pdf.Add(dottSep);

                pdf.Add(new Paragraph($"Date: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{log.Date.ToString()}"));
                pdf.Add(new Paragraph($"Distance: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{Convert.ToString(log.Distance)}"));
                pdf.Add(new Paragraph($"Time Taken: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{log.TotalTime.ToString()}"));
                pdf.Add(new Paragraph($"Rating: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{Convert.ToString(log.Rating)}"));
                pdf.Add(new Paragraph($"Vehicle: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{log.Vehicle}"));
                pdf.Add(new Paragraph($"Velocity: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{Convert.ToString(log.Velocity)}km/h"));
                pdf.Add(new Paragraph($"Steep: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{log.SteepSections.ToString()}"));
                pdf.Add(new Paragraph($"Scenic: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{log.IsScenic.ToString()}"));
                pdf.Add(new Paragraph($"DifficultyLevel: ").Add(new Tab()).AddTabStops(new TabStop(1000, TabAlignment.RIGHT)).Add($"{Convert.ToString(log.DifficultyLevel)}"));
                pdf.Add(new Paragraph($"Report: ").Add("\n").Add($"{log.Report}"));
                first = false;
            }
            pdf.Close();
        }
    }
}
