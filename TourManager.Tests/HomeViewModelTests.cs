using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManager.BusinessLayer;
using TourManager.Stores;
using TourManager.ViewModels;
using TourManagerModels;

namespace TourManager.Tests
{
    [TestFixture]
    public class HomeViewModelTests
    {
        Mock<ITourItemFactory> mockFactory = new Mock<ITourItemFactory>();
        string pathTestImg = @"C:\Users\mbern\Downloads\FH-Stuff\4.Semester\SWE2\TourManagerApplication\TestIMG\testImage.jpg";

        List<Log> fakeLogs = new List<Log>();        

        [Test]
        public void SetNewBitmapImage_ReturnsImage_SeeIfReturnValueNotEmpty()
        {
            NavigationStore falseNav = new NavigationStore();
            HomeViewModel homeVM = new HomeViewModel(falseNav, mockFactory.Object);

            string path = pathTestImg;
            homeVM.RouteImage = null;

            homeVM.SetNewBitmapImage(path);

            Assert.That(homeVM.RouteImage != null);            
        }

        [Test]
        public void SetNewBitmapImage_ReturnsImage_ErrorCaseImageStaysNull()
        {
            NavigationStore falseNav = new NavigationStore();
            HomeViewModel homeVM = new HomeViewModel(falseNav, mockFactory.Object);

            string path = @"C:\Users\FalsePath";
            homeVM.RouteImage = null;

            homeVM.SetNewBitmapImage(path); 

            Assert.That(homeVM.RouteImage == null);
        }

        //[Test]
        //public void GetLogsFromName_ReturnsListOfLogs_PopulatesListFromBusinessLayerInformation()
        //{
        //    NavigationStore falseNav = new NavigationStore();
        //    HomeViewModel homeVM = new HomeViewModel(falseNav, mockFactory.Object);

        //    string fakeTourName = "dummy1";
        //    Log dummy1 = new Log(fakeTourName, DateTime.Now, "Rport", 3.6, TimeSpan.FromMinutes(30), 4, "vehic", 3.9, true, false, 4);
        //    Log dummy2 = new Log(fakeTourName, DateTime.Now, "NewRport", 6.3, TimeSpan.FromMinutes(40), 4, "bike", 9.3, false, false, 2);
        //    Log dummy3 = new Log(fakeTourName, DateTime.Now, "OldTourBus", 8.3, TimeSpan.FromMinutes(150), 4, "bus", 5.5, false, true, 1);
        //    fakeLogs.Add(dummy1); fakeLogs.Add(dummy2); fakeLogs.Add(dummy3);
        //    mockFactory.Setup(m => m.GetLogs(It.IsAny<string>())).Returns((IEnumerable<Log>)Task.FromResult<List<Log>>((List<Log>)fakeLogs));           

        //    homeVM.GetLogsFromName(fakeTourName);

        //    Assert.That(homeVM.Logs.Count == 3);
        //    Assert.That(homeVM.Logs.SequenceEqual(fakeLogs));            
        //}
    }
}
