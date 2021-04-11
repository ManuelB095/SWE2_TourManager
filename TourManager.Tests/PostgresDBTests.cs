using System;
using System.Collections.Generic;
using NUnit.Framework;
using TourManager.DAL;
using TourManagerModels;

namespace TourManager.Tests
{
    [TestFixture]
    public class PostgresDBTests
    {
        [Test]
        public void GetItems_PostgresDB_returnsListTours()
        {
            PostgresDB db = new PostgresDB();
            List<Tour> test = db.GetItems();

            Assert.That(test.Count != 0);
        }
    }
}
