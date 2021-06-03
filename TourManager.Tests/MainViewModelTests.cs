using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TourManager.ViewModels;
using TourManager.Stores;
using TourManagerModels;

namespace TourManager.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        [Test]
        public void Constructor_FillTourItems_checkForEntries()
        {
            NavigationStore falseNav = new NavigationStore();
            MainViewModel mainVM = new MainViewModel(falseNav);
            Assert.That(mainVM.TourItems.Count != 0);
            // Not a good test, bc how can i know how many items get returned by the database?
            //Assert.That(mainVM.TourItems.Count == 3); 
        }

        [Test]
        public void SearchCommand_returnsOneResult() 
        {
            Tour testTour = new Tour("Bergweg nach Mordor", "empty", "empty", 0);

            NavigationStore falseNav = new NavigationStore();
            MainViewModel mainVM = new MainViewModel(falseNav);

            mainVM.SearchName = "Mordor";
            mainVM.SearchCommand.Execute(null);
            Assert.That(mainVM.TourItems.Count == 1);
            Assert.That(mainVM.TourItems[0].Name == testTour.Name);
        }

        [Test]
        public void SearchCommand_returnsZeroResult()
        {
            Tour testTour = new Tour("Bergweg nach Mordor", "empty", "empty", 0);

            NavigationStore falseNav = new NavigationStore();
            MainViewModel mainVM = new MainViewModel(falseNav);

            mainVM.SearchName = "brzlbrz";
            mainVM.SearchCommand.Execute(null);
            Assert.That(mainVM.TourItems.Count == 0);
        }
    }
}
