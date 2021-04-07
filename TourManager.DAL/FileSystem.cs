using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;

namespace TourManager.DAL
{
    public class FileSystem : IDataAccess
    {
        private string filePath;

        public FileSystem()
        {
            // TO DO: Get filePath from configFile
            this.filePath = "PLACEHOLDER";
        }

        public List<Tour> GetItems()
        {
            // TO DO : Get Information from File
            // GENERALLY NOT TOURS. THESE ARE JUST PLACEHOLDERS!!
            List<Tour> allTourItems = new List<Tour>();
            allTourItems.Add(new Tour("Gemäßigte Stadtroute",
                                    "Eine aussichtsreiche Zugfahrt, bei der selbst Einsteiger und Babys mitmachen können.",
                                    "Von Wien nach Linz in 1:15h", 3.5));
            allTourItems.Add(new Tour("Bergweg nach Mordor",
                                    "Man geht nicht einfach nach Mordor heißt es im Volksmund. " +
                                    "Doch das hielt die Wunderpolinger Wanderspatzen nicht auf " +
                                    "und sie haben trotzdem eine Route angelegt. " +
                                    "Treten auch sie in die Fußstapfen von Frodo und Co mit " +
                                    "dieser wunderschönen Bergtour",
                                    "Von Wien nach Mordor in 96:45", 365));
            allTourItems.Add(new Tour("Kurz zum Spar",
                                    "Haben sie es satt, dass wiedermal kein Essen im Kühlschrank ist? Das muss nicht sein! Mit unserer Kurz-Zum-Spar Wanderroute kommen sie " +
                                    "direkt an jedermanns liebstem Lebensmittelfachhändler vorbei!",
                                    "Zum Spar in weiß ich nicht, 5-10 Minuten?", 1));
            return allTourItems;
        }
    }
}
