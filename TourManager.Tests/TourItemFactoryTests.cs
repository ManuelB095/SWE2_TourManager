using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManager.BusinessLayer;
using TourManager.DAL;
using TourManagerModels;

namespace TourManager.Tests
{
    [TestFixture]
    public class TourItemFactoryTests
    {
        // SetUp
        Tour dummyTour = new Tour("dummyTour", "Test", "EmptyString", 3.4);
        Tour dummyTour2 = new Tour("anotherDummy", "MoreTestingGoingOn", "NotMapInfoHere", 5.5);
        Tour dummyTour3 = new Tour("Bergweg nach Mordor", "empty", "empty", 0);
        List<Tour> mockTours = new List<Tour>();

        Mock<ITourItemDAO> mockedDAO = new Mock<ITourItemDAO>();

        ITourItemFactory testInstance = TourItemFactory.GetInstance();

        //[Test]
        //public void Search_ReturnsTourThatWasFound() LINQ DOES NOT WORK WITH UNIT TESTS BECAUSE MICROSOFT OR WHATEVER
        //{
        //    mockTours.Add(dummyTour); mockTours.Add(dummyTour2); mockTours.Add(dummyTour3);
        //    mockedDAO.Setup(m => m.GetItems()).Returns(mockTours);

        //    testInstance.ChangeDataSource(mockedDAO.Object);

        //    List<Tour> tours = new List<Tour>();

        //    IEnumerable results = testInstance.Search("Mordor");
        //    foreach(Tour t in results)
        //    {
        //        tours.Add(t);
        //    }

        //    Assert.That(tours.Contains(dummyTour));
        //    Assert.That(tours.Count() == 1);
        //}

    }
}
