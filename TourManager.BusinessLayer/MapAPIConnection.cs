using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using TourManager.DAL;

namespace TourManager.BusinessLayer
{
    public class MapAPIConnection
    {
        // TODO
        // Get key from config
        // Get imges folder from config
        // key = fNyu2LJqK0FYbcJSgAKzTUzSARvIEKAD

        // Second TODO
        // Save Params from First Request
        // Send SecRequest with savedParams
        // Save map as image file and
        // ... persist location in DB under routeinformation

        //Get those next four from config...
        public string URL_ROUTE { get; set; } = "http://www.mapquestapi.com/directions/v2/route";

        public string KEY = ConfigurationManager.AppSettings["MAP_API_KEY"];
        public string URL_STATIC_MAP { get; set; } = "http://www.mapquestapi.com/staticmap/v5/map"; //key=fNyu2LJqK0FYbcJSgAKzTUzSARvIEKAD&size=640,480&defaultMarker=none&zoom=11&rand=737758036&session=6085a120-015f-4ee4-02b4-35c2-0e91b2526d11&boundingBox=48.41787,%2015.601635,%2048.192204,%2016.41643";

        public string IMG_PATH { get; set; } = ConfigurationManager.AppSettings["MAPS_FILE_PATH"]; //@"C:\Users\mbern\Downloads\FH-Stuff\4.Semester\SWE2\TourManagerApplication\Maps\"
      
        public string ParametrizedRouteURL { get; set; }

        // Get these from FIRST Request
        public string ParametrizedMapURL { get; set; }
        public string Dimensions { get; set; } = "620,480";
        public string sessionID { get; set; }

        public double distance { get; set; }
        public string ul_lat { get; set; } //ul = Upper Left; lat = Latitude
        public string ul_lng { get; set; } //lng = Longitude

        public string lr_lat { get; set; } //lr = Lower Right
        public string lr_lng { get; set; }

        public string ResultingFilePath { get; private set; }
        
        private IFileAccess imgManager;

        public MapAPIConnection()
        {
            imgManager = new FileHandler();
        }

        public void HandleMapQuestRequest(string from, string to)
        {
            CreateURLForRouteRequest(from,to);
            sendRouteRequest();
            CreateURLForMapRequest();
            sendMapRequest();
        }


        // Sends First Request to obtain Route Information.
        private void sendRouteRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.ParametrizedRouteURL);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                JObject jo = JObject.Parse(json);

                //Get the appropriate Information from the resulting JSON
                this.sessionID = jo["route"]["sessionId"].ToString();
                this.distance = Convert.ToDouble(jo["route"]["distance"].ToString());
                this.ul_lat = jo["route"]["boundingBox"]["ul"]["lat"].ToString();
                this.ul_lng = jo["route"]["boundingBox"]["ul"]["lng"].ToString();
                this.lr_lat = jo["route"]["boundingBox"]["lr"]["lat"].ToString();
                this.lr_lng = jo["route"]["boundingBox"]["lr"]["lng"].ToString();
                this.ul_lat = this.ul_lat.Replace(",", ".");
                this.ul_lng = this.ul_lng.Replace(",", ".");
                this.lr_lat = this.lr_lat.Replace(",", ".");
                this.lr_lng = this.lr_lng.Replace(",", ".");
            }
        }

        // With data from First Request: Try to obtain static map and save as file.
        private void sendMapRequest()
        {
            HttpWebRequest mapRequest = (HttpWebRequest)WebRequest.Create(this.ParametrizedMapURL);
            using (HttpWebResponse mapResponse = (HttpWebResponse)mapRequest.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(mapResponse.GetResponseStream()))
                {
                    Byte[] imgBytes = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    this.ResultingFilePath = imgManager.NewFileEntry(IMG_PATH, imgBytes);                   
                }
            }
        }

        private void CreateURLForRouteRequest(string from, string to)
        {
            this.ParametrizedRouteURL = this.URL_ROUTE; // http://www.mapquestapi.com/directions/v2/route
            this.ParametrizedRouteURL += "?key=";
            this.ParametrizedRouteURL += this.KEY;
            this.ParametrizedRouteURL += "&from=";
            this.ParametrizedRouteURL += from;
            this.ParametrizedRouteURL += "&to=";
            this.ParametrizedRouteURL += to;
        }

        private void CreateURLForMapRequest()
        {
            this.ParametrizedMapURL = this.URL_STATIC_MAP; // http://www.mapquestapi.com/staticmap/v5/map
            this.ParametrizedMapURL += "?key=";
            this.ParametrizedMapURL += this.KEY;
            this.ParametrizedMapURL += "&session=";
            this.ParametrizedMapURL += this.sessionID;
            this.ParametrizedMapURL += "&boundingBox=";
            this.ParametrizedMapURL += this.ul_lat;
            this.ParametrizedMapURL += ",";
            this.ParametrizedMapURL += this.ul_lng;
            this.ParametrizedMapURL += ",";
            this.ParametrizedMapURL += this.lr_lat;
            this.ParametrizedMapURL += ",";
            this.ParametrizedMapURL += this.lr_lng; 
            this.ParametrizedMapURL += ",";
            this.ParametrizedMapURL += "&size=";
            this.ParametrizedMapURL += this.Dimensions;
        }
    }
}

