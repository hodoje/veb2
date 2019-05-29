using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class Vehicle
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int TransportationLineId { get; set; }
        public TransportationLine TransportationLine { get; set; }
    }
}