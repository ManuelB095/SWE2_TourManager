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
    public class CreateToursViewModelTests
    {
        Mock<ITourItemFactory> mockFactory = new Mock<ITourItemFactory>();

        [Test]
        public void GetErrorFromProperty_ReturnsErrorString_TooManyCharacters()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateToursViewModel createToursVM = new CreateToursViewModel(falseNav, mockFactory.Object);
            createToursVM.Error = "";
            createToursVM.tourName = "ThisTourNameIsWayyyyyyTooLongAndShouldThereforeThrowAnErrorInTheApplication";

            createToursVM.GetErrorForProperty("tourName");

            Assert.That(createToursVM.Error == "Tour Name cannot be longer than 40 chars!");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsErrorString_ContainsNumericalValues()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateToursViewModel createToursVM = new CreateToursViewModel(falseNav, mockFactory.Object);
            createToursVM.Error = "";
            createToursVM.tourName = "This3CanNotStandInAnHonestApp!";

            createToursVM.GetErrorForProperty("tourName");

            Assert.That(createToursVM.Error == "Tour Name can only consist of characters A-Z or a-z!");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsEmptyErrorString_ValidInput()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateToursViewModel createToursVM = new CreateToursViewModel(falseNav, mockFactory.Object);
            createToursVM.Error = "";
            createToursVM.tourName = "Dachstein-Route";

            createToursVM.GetErrorForProperty("tourName");

            Assert.That(createToursVM.Error == "");

        }

        [Test]
        public void GetErrorFromProperty_ReturnsErrorString_TourDescriptionIsTooLong()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateToursViewModel createToursVM = new CreateToursViewModel(falseNav, mockFactory.Object);
            createToursVM.Error = "";
            createToursVM.tourDescription = "Did you ever hear the Tragedy of Darth Plagueis the wise? I thought not. It's not a story the Jedi would tell you. It's a Sith legend. Darth Plagueis was a Dark Lord of the Sith, so powerful and so wise he could use the Force to influence the midichlorians to create life... He had such a knowledge of the dark side that he could even keep the ones he cared about from dying. The dark side of the Force is a pathway to many abilities some consider to be unnatural. He became so powerful... the only thing he was afraid of was losing his power, which eventually, of course, he did. Unfortunately, he taught his apprentice everything he knew, then his apprentice killed him in his sleep. It's ironic he could save others from death, but not himself.";

            createToursVM.GetErrorForProperty("TourDescription");

            Assert.That(createToursVM.Error == "Tour Description cannot be longer than 250 chars!");
        }

        [Test]
        public void GetErrorFromProperty_ReturnsEmptyErrorString_TourDescriptionValid()
        {
            NavigationStore falseNav = new NavigationStore();
            CreateToursViewModel createToursVM = new CreateToursViewModel(falseNav, mockFactory.Object);
            createToursVM.Error = "";
            createToursVM.tourDescription = "Eine deutlich lange, aber dennoch kurze und passende Beschreibung der Dachsteinroute. \n Wie man sieht, auch über mehrere Zeilen!";

            createToursVM.GetErrorForProperty("TourDescription");

            Assert.That(createToursVM.Error == "");
        }

    }
}
