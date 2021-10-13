using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManager.BusinessLayer;
using TourManagerModels;

namespace TourManager.Tests
{
    [TestFixture]
    public class ReportGeneratorTests
    {
        string testPathReport = @"C:\Users\mbern\Downloads\FH-Stuff\4.Semester\SWE2\TourManagerApplication\TestReports\";
        string pathTestImg = @"C:\Users\mbern\Downloads\FH-Stuff\4.Semester\SWE2\TourManagerApplication\TestIMG\testImage.jpg";

        [Test]
        public void GenerateTourReport_ChecksSuccessfulGeneration_TourHasImage()
        {
            Tour testTour = new Tour("dummyTour", "dummyDescript", pathTestImg, 4.5);

            IReportGenerator testRepo = ReportGenerator.GetInstance();
            testRepo.ChangeOutputPath(testPathReport);
            testRepo.GenerateTourReport(testTour);

            string expectedFileName = testRepo.GetPath() + testTour.Name + ".pdf";

            Assert.That(File.Exists(expectedFileName));
            Assert.That(testRepo.ImgExists == true); // tour had an image of its own


            //Clean-Up
            if (File.Exists(expectedFileName))
                File.Delete(expectedFileName);
        }

        [Test]
        public void GenerateTourReport_ChecksUnSuccessfulGeneration_TourHasNoImage()
        {
            Tour testTour = new Tour("dummyTour", "dummyDescript", "Empty", 4.5);

            IReportGenerator testRepo = ReportGenerator.GetInstance();
            testRepo.ChangeOutputPath(testPathReport);
            testRepo.GenerateTourReport(testTour);

            string expectedFileName = testRepo.GetPath() + testTour.Name + ".pdf";

            Assert.That(File.Exists(expectedFileName));
            Assert.That(testRepo.ImgExists == false); // tour had no image of its own

            //Clean-Up
            if (File.Exists(expectedFileName))
                File.Delete(expectedFileName);
        }






    }
}
