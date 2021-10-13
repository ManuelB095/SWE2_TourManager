using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TourManager.ViewModels;
using TourManager.Stores;
using TourManagerModels;
using Moq;
using TourManager.BusinessLayer;

namespace TourManager.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        // SetUp
        Tour dummyTour = new Tour("dummyTour", "Test", "EmptyString", 3.4);
        Tour dummyTour2 = new Tour("anotherDummy", "MoreTestingGoingOn", "NotMapInfoHere", 5.5);
        Tour dummyTour3 = new Tour("Bergweg nach Mordor", "empty", "empty", 0);

        Mock<ITourItemFactory> mockFactory = new Mock<ITourItemFactory>();
        List<Tour> mockTours = new List<Tour>();

        [Test]
        public void SearchCommand_returnsOneResult() 
        {
            //SetUp
            List<Tour> searchResults = new List<Tour>();
            searchResults.Add(dummyTour3);

            mockTours.Add(dummyTour); mockTours.Add(dummyTour2); mockTours.Add(dummyTour3);
            mockFactory.Setup(m => m.GetTours()).Returns(mockTours);
            mockFactory.Setup(m => m.Search(It.IsAny<string>(), It.IsAny<bool>())).Returns(searchResults);

            NavigationStore falseNav = new NavigationStore();
            MainViewModel mainVM = new MainViewModel(falseNav, mockFactory.Object);            

            //Test
            mainVM.SearchName = "Mordor";
            mainVM.SearchCommand.Execute(null);

            //Assert
            Assert.That(mainVM.TourItems.Count == 1);
            Assert.That(mainVM.TourItems[0].Name == dummyTour3.Name);
        }

        [Test]
        public void SearchCommand_returnsZeroResult()
        {
            //SetUp
            List<Tour> searchResults = new List<Tour>();

            mockTours.Add(dummyTour); mockTours.Add(dummyTour2); mockTours.Add(dummyTour3);
            mockFactory.Setup(m => m.GetTours()).Returns(mockTours);
            mockFactory.Setup(m => m.Search(It.IsAny<string>(), It.IsAny<bool>())).Returns(searchResults);

            NavigationStore falseNav = new NavigationStore();
            MainViewModel mainVM = new MainViewModel(falseNav, mockFactory.Object);            

            mainVM.SearchName = "brzlbrz";
            mainVM.SearchCommand.Execute(null);
            Assert.That(mainVM.TourItems.Count == 0);
        }

        [Test]
        public void RefreshCommand_returnsUpdatedListWhenNewEntryWasMade()
        {
            List<Tour> searchResults = new List<Tour>();
            searchResults.Add(dummyTour3);

            mockTours.Add(dummyTour); mockTours.Add(dummyTour2); mockTours.Add(dummyTour3);
            mockFactory.Setup(m => m.GetTours()).Returns(mockTours);
            mockFactory.Setup(m => m.Search(It.IsAny<string>(), It.IsAny<bool>())).Returns(searchResults);

            NavigationStore falseNav = new NavigationStore();
            MainViewModel mainVM = new MainViewModel(falseNav, mockFactory.Object);
            mainVM.SearchName = "Mordor";
            mainVM.SearchCommand.Execute("");

            mainVM.RefreshCommand.Execute("");

            Assert.That(mainVM.SearchName == "");
            Assert.That(mainVM.TourItems.Count == mockTours.Count);
        }        
    }
}
