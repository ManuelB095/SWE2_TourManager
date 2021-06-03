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
        private PostgresDB Dbase = new PostgresDB();

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

            Image img = new Image(ImageDataFactory.Create(t.RouteInformation));
            img.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            img.SetHeight(120);
            img.SetHeight(180);

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
