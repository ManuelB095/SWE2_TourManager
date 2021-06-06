using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TourManager.BusinessLayer;
using TourManager.DAL;

namespace TourManager.Tests
{
    [TestFixture]
    public class MapAPIConnectionTest
    {
        [Test]
        public void SendRequest_returnByteArray() // WARNING: This makes two API Calls. Beware of the free 15.000 Calls per Month Limit!
        {
            MapAPIConnection test = new MapAPIConnection();
            test.HandleMapQuestRequest("Wien", "Graz");

            // Test
            Assert.That(File.Exists(test.ResultingFilePath));

            //Clean-Up
            if (File.Exists(test.ResultingFilePath))
                File.Delete(test.ResultingFilePath);
        }
    }
}
