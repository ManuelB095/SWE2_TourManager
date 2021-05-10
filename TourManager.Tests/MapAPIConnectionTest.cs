using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TourManager.BusinessLayer;

namespace TourManager.Tests
{
    [TestFixture]
    public class MapAPIConnectionTest
    {
        [Test]
        public void SendRequest_returnByteArray()
        {
            MapAPIConnection test = new MapAPIConnection();
            test.HandleMapQuestRequest("Wien", "Graz");

            // Placeholder till i have something better to check for (i.e. if file exists)
            Assert.That(true);
        }
    }
}
