using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using jonasalden_travelplanner.Models;

namespace jonasalden_travelplanner.ViewModels
{
    public class TicketsViewModel
    {
        [Required]
        public string DepName { get; set; }
        public DateTime DepTime { get; set; }

        public string DestId { get; set; }
        [Required]
        public string DestName { get; set; }
        public DateTime DestTime { get; set; }
        public string DestLon { get; set; }
        public string DestLat { get; set; }

        public decimal Celsius { get; set; }
        public string ForecastName { get; set; }
    }
}