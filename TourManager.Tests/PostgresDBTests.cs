using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using TourManager.DAL;
using TourManagerModels;

namespace TourManager.Tests
{
    [TestFixture]
    public class PostgresDBTests // Questionable if database should even be tested
    {
        [SetUp]
        public void SetUpFalseTours()
        {
            // Getting allTourItems over here does not work. Could not find out why that is.
        }

        [Test]
        public void GetItems_returnsListTours() // This is OBSOLETE AS SOON AS AddTour() is implemented and tested via moq below
        {
            PostgresDB postgres = new PostgresDB();
            List<Tour> test = postgres.GetItems();
            Assert.That(test.Count != 0); //Bit redundant maybe?
            Assert.That(test.Count == 3);
            Assert.That(test[0].Name == "Gem‰ﬂigte Stadtroute");
            Assert.That(test[1].Name == "Bergweg nach Mordor");
            Assert.That(test[2].Name == "Kurz zum Spar");
        }

        [Test]
        public void GetItemsWithMoq_returnsListTours()
        {
            List<Tour> allTourItems = new List<Tour>();
            allTourItems.Add(new Tour("Gem‰ﬂigte Stadtroute",
                                    "Eine aussichtsreiche Zugfahrt, bei der selbst Einsteiger und Babys mitmachen kˆnnen.",
                                    "Von Wien nach Linz in 1:15h", 3.5));
            allTourItems.Add(new Tour("Bergweg nach Mordor",
                                    "Man geht nicht einfach nach Mordor heiﬂt es im Volksmund. " +
                                    "Doch das hielt die Wunderpolinger Wanderspatzen nicht auf " +
                                    "und sie haben trotzdem eine Route angelegt. " +
                                    "Treten auch sie in die Fuﬂstapfen von Frodo und Co mit " +
                                    "dieser wunderschˆnen Bergtour",
                                    "Von Wien nach Mordor in 96:45", 365));
            allTourItems.Add(new Tour("Kurz zum Spar",
                                    "Haben sie es satt, dass wiedermal kein Essen im K¸hlschrank ist? Das muss nicht sein! Mit unserer Kurz-Zum-Spar Wanderroute kommen sie " +
                                    "direkt an jedermanns liebstem Lebensmittelfachh‰ndler vorbei!",
                                    "Zum Spar in weiﬂ ich nicht, 5-10 Minuten?", 1));

            // Mock Database with Pre-Defined-Tours, so it does not have to rely on database
            var mock = new Mock<IDataAccess>();
            mock.Setup(x => x.GetItems()).Returns(allTourItems);

            List<Tour> test = mock.Object.GetItems();

            Assert.That(test.Count != 0); //Bit redundant maybe?
            Assert.That(test.Count == 3);
            Assert.That(test[0].Name == "Gem‰ﬂigte Stadtroute");
            Assert.That(test[1].Name == "Bergweg nach Mordor");
            Assert.That(test[2].Name == "Kurz zum Spar");
        }

        // Needs an Add Method First !

        //[Test]
        //public void GetTourByName_returnsTour()
        //{
        //    List<Tour> allTourItems = new List<Tour>();
        //    allTourItems.Add(new Tour("Gem‰ﬂigte Stadtroute","","", 3.5));
        //    allTourItems.Add(new Tour("Bergweg nach Mordor","","", 365));
        //    allTourItems.Add(new Tour("Kurz zum Spar","","", 1));

        //    // Mock Database with Pre-Defined-Tours, so it does not have to rely on database
        //    var mock = new Mock<IDataAccess>();
        //    mock.Setup(x => x.GetItems()).Returns(allTourItems);

        //    List<Tour> test = mock.Object.GetItems();

        //    Assert.That(test.Count != 0); //Bit redundant maybe?
        //    Assert.That(test.Count == 3);
        //    Assert.That(test[0].Name == "Gem‰ﬂigte Stadtroute");
        //    Assert.That(test[1].Name == "Bergweg nach Mordor");
        //    Assert.That(test[2].Name == "Kurz zum Spar");


        //}
    }
}
