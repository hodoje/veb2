using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    public class TransportationLineRoute
    {
        public int Id { get; set; }
        public int RoutePoint { get; set; }
        public int TransporationLineId { get; set; }
        public virtual TransportationLine TransporationLine { get; set; }
        public int StationId { get; set; }
        public virtual Station Station { get; set; }
    }
}