using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TourManagerModels;
using Npgsql;

namespace TourManager.DAL
{
    public class PostgresDB : IDataAccess
    {
        private string connString;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PostgresDB()
        {
            connString = ConfigurationManager.AppSettings["DB_CONN_STRING"];
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

        public List<Log> GetLogs(string tourName)
        {
            List<Log> LogList = new List<Log>();

            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();

            using (var cmd = new NpgsqlCommand("Select date,report,distance,totaltime,rating,vehicle,velocity,steepsections,scenic,difficultylevel From Log Where tourname = @tourname", conn))
            {
                cmd.Parameters.AddWithValue("tourname", tourName);
                cmd.Prepare();
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    log.Error("Could not get any logs for tour: " + tourName);
                }

                while (reader.Read())
                {
                    Log dummyLog = new Log();
                    dummyLog.Date = (DateTime)reader["date"];
                    dummyLog.Report = (string)reader["report"];
                    dummyLog.Distance = Convert.ToDouble(reader["distance"]);
                    dummyLog.TotalTime = (TimeSpan)reader["totaltime"];
                    dummyLog.Rating = Convert.ToDouble(reader["rating"]);
                    dummyLog.Vehicle = (string)reader["vehicle"];
                    dummyLog.SteepSections = (bool)reader["steepsections"];
                    dummyLog.Velocity = Convert.ToDouble(reader["velocity"]);
                    dummyLog.Velocity = Math.Round(dummyLog.Velocity, 2);
                    dummyLog.IsScenic = (bool)reader["scenic"];
                    dummyLog.DifficultyLevel = Convert.ToInt32(reader["difficultylevel"]);
                    LogList.Add(dummyLog);
                }
                conn.Close();
                return LogList;
            }
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
                conn.Close();
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
                conn.Close();
                return names;
            }
        }

        public void AddTour(string name, string description, string routeInfo, double distance)
        {
            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();
            using (var cmd = new NpgsqlCommand("Insert Into Tour(tourname,description,routeinformation,tourdistance) Values(@name,@desc,@route,@dist)", conn))
            {
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("desc", description);
                cmd.Parameters.AddWithValue("route", routeInfo);
                cmd.Parameters.AddWithValue("dist", distance);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void AddLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel)
        {           
            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();
            using (var cmd = new NpgsqlCommand("Insert Into Log(date,tourname,report,distance,totaltime,rating,vehicle,velocity,steepsections,scenic,difficultylevel)" +
            "Values(@date,@tourname,@report,@distance,@totaltime,@rating,@vehicle,@velocity,@steepsections,@scenic,@difficultylevel); ", conn))
            {
                double velocity = (logDistance*1000) / (double)logTotalTime.TotalSeconds;
                velocity = Math.Round(velocity, 2);

                cmd.Parameters.AddWithValue("date", logDate);
                cmd.Parameters.AddWithValue("tourname", tourName);
                cmd.Parameters.AddWithValue("report", report);
                cmd.Parameters.AddWithValue("distance", logDistance);
                cmd.Parameters.AddWithValue("totaltime", logTotalTime);
                cmd.Parameters.AddWithValue("rating", LogRating);
                cmd.Parameters.AddWithValue("vehicle", vehicle);
                cmd.Parameters.AddWithValue("velocity", velocity);
                cmd.Parameters.AddWithValue("steepsections", steepSections);
                cmd.Parameters.AddWithValue("scenic", scenic);
                cmd.Parameters.AddWithValue("difficultylevel", difficultyLevel);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void UpdateTour(string tourName, string description, string routeInfo, double distance)
        {
            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();
            using (var cmd = new NpgsqlCommand("Update tour Set description = @description, routeinformation = @routeinformation, tourdistance = @tourdistance Where tourname = @tourname", conn))
            {
                cmd.Parameters.AddWithValue("tourname", tourName);
                cmd.Parameters.AddWithValue("description", description);
                cmd.Parameters.AddWithValue("routeinformation", routeInfo);
                cmd.Parameters.AddWithValue("tourdistance", distance);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void DeleteTour(string tourName)
        {
            //Delete All corresponding Logs first (Foreign Key restraint!)
            DeleteLogsFromTour(tourName);

            //Then Delete Tours
            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();
            using (var cmd = new NpgsqlCommand("Delete From tour Where tourname = @tourname", conn))
            {
                cmd.Parameters.AddWithValue("tourname", tourName);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void DeleteLogsFromTour(string tourName)
        {
            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();
            using (var cmd = new NpgsqlCommand("Delete From log Where tourname = @tourname", conn))
            {
                cmd.Parameters.AddWithValue("tourname", tourName);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void UpdateLog(string tourName, DateTime logDate, double logDistance, TimeSpan logTotalTime, double LogRating, string vehicle, string report, bool steepSections, bool scenic, int difficultyLevel)
        {
            using var conn = new NpgsqlConnection(this.connString);
            conn.Open();
            using (var cmd = new NpgsqlCommand("Update log Set report = @report, distance = @distance, totaltime = @totaltime, rating = @rating, vehicle = @vehicle, velocity = @velocity, steepsections = @steepsections, scenic = @scenic, difficultylevel = @difficultylevel Where date = @date And tourname = @tourname", conn))
            {
                double velocity = (logDistance * 1000) / (double)logTotalTime.TotalSeconds;
                velocity = Math.Round(velocity, 2);

                cmd.Parameters.AddWithValue("date", logDate);
                cmd.Parameters.AddWithValue("tourname", tourName);
                cmd.Parameters.AddWithValue("report", report);
                cmd.Parameters.AddWithValue("distance", logDistance);
                cmd.Parameters.AddWithValue("totaltime", logTotalTime);
                cmd.Parameters.AddWithValue("rating", LogRating);
                cmd.Parameters.AddWithValue("vehicle", vehicle);
                cmd.Parameters.AddWithValue("velocity", velocity);
                cmd.Parameters.AddWithValue("steepsections", steepSections);
                cmd.Parameters.AddWithValue("scenic", scenic);
                cmd.Parameters.AddWithValue("difficultylevel", difficultyLevel);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
