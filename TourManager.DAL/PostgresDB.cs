using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourManagerModels;
using Npgsql;

namespace TourManager.DAL
{
    public class PostgresDB : IDataAccess
    {
        private string connString;

        public PostgresDB()
        {
            // TO DO: Add connString from config File
            connString = "Host=localhost;Username=postgres;Password=postgres;Database=TourManager";
        }

        public List<Tour> GetItems()
        {
            List<string> tournames = GetAllTourNames();
            List<Tour> items = new List<Tour>();
            foreach(string name in tournames)
            {
                items.Add(GetTourByName(name));
            }
            return items;
        }

        public Tour GetTourByName(string _tourName)
        {
            Tour dummyTour = new Tour("", "", "", 0);
            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();

            using (var cmd = new NpgsqlCommand("Select tourname, description, routeinformation, tourdistance From Tour Where tourname = @tourname", conn))
            {
                cmd.Parameters.AddWithValue("tourname", _tourName);
                cmd.Prepare();
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    string err = "ERROR: GetTourByName could not find tourname of '";
                    err += _tourName;
                    err += "' !";
                    throw new InvalidOperationException(err);
                }

                while (reader.Read())
                {
                    dummyTour.Name = (string)reader["tourname"];
                    dummyTour.TourDescription = (string)reader["description"];
                    dummyTour.RouteInformation = (string)reader["routeinformation"];
                    string distance = reader["tourdistance"].ToString();
                    dummyTour.TourDistance = Convert.ToDouble(distance);
                }
                return dummyTour;
            }
        }

        public List<string> GetAllTourNames()
        {
            List<string> names = new List<string>();
            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();

            using (var cmd = new NpgsqlCommand("Select tourname From Tour", conn))
            {
                cmd.Prepare();
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        names.Add((string)reader[i]);
                    }
                }
                return names;
            }
        }
    }
}
