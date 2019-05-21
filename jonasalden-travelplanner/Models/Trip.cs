using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jonasalden_travelplanner.Models
{
    public class Trip
    {
        // Department
        public string DepId { get; set; }
        public string DepName { get; set; }
        public DateTime DepTime { get; set; }
        
        // Destination
        public string DestId { get; set; }
        public string DestName { get; set; }
        public string DestLon { get; set; }
        public string DestLat { get; set; }
        public DateTime DestTime { get; set; }

        // Weather
        public int Forecast { get; set; }
        public string ForecastName { get; set; }
        public decimal Celsius { get; set; }
    }
}