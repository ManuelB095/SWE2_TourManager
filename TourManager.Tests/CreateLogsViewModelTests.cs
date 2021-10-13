using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TourManager.BusinessLayer;
using TourManager.Stores;
using TourManager.ViewModels;

namespace TourManager.Tests
{
    [TestFixture]
    public class CreateLogsViewModelTests
    {
        Mock<ITourItemFactory> mockFactory = new Mock<ITourItemFactory>();

        [Test]
        public void GetErrorFromProperty_ReturnsErrorString_LogDistanceUnderZero()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogDistance = "-0.1";

            createLogsVM.GetErrorForProperty("LogDistance");

            Assert.That(createLogsVM.Error == "Log Distance can not be smaller than Zero!");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsErrorString_LogDistanceContainsAlphabeticals()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogDistance = "3.a";

            createLogsVM.GetErrorForProperty("LogDistance");

            Assert.That(createLogsVM.Error == "Log Distance can only use integer or decimal numbers! Use only numbers from 0-9!");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsEmptyErrorString_DynamicallyRecognizeFalseSeparator()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogDistance = "3,14";

            createLogsVM.GetErrorForProperty("LogDistance");

            Assert.That(createLogsVM.Error == "");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsEmptyErrorString_LogDistanceIntegersOnly()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogDistance = "1234";

            createLogsVM.GetErrorForProperty("LogDistance");

            Assert.That(createLogsVM.Error == "");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsEmptyErrorString_LogDistanceConventionalDouble()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogDistance = "3.14";

            createLogsVM.GetErrorForProperty("LogDistance");

            Assert.That(createLogsVM.Error == "");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsErrorString_RatingCanNotBeHigher10()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogRating = "11";

            createLogsVM.GetErrorForProperty("LogRating");

            Assert.That(createLogsVM.Error == "Rating cannot be higher than 10");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsErrorString_RatingNegativeNumber()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogRating = "-4";

            createLogsVM.GetErrorForProperty("LogRating");

            Assert.That(createLogsVM.Error == "Rating must be a positive integer value");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsEmptyErrorString_CorrectRating()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogRating = "2";

            createLogsVM.GetErrorForProperty("LogRating");

            Assert.That(createLogsVM.Error == "");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsErrorString_OnlyAcceptsWholeMinutes()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogTotalTime = "40,5";

            createLogsVM.GetErrorForProperty("LogTotalTime");

            Assert.That(createLogsVM.Error == "Total Time can only consist of nonnegative whole numbers!");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsEmptyErrorString()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateLogsViewModel createLogsVM = new CreateLogsViewModel(falseNav, mockFactory.Object);
            createLogsVM.Error = "";
            createLogsVM.LogTotalTime = "40";

            createLogsVM.GetErrorForProperty("LogTotalTime");

            Assert.That(createLogsVM.Error == "");
        }

    }
}
