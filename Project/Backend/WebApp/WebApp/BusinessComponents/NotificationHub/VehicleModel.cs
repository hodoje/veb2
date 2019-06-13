using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.BusinessComponents.NotificationHub
{
    public class VehicleModel
    {
        public int LineNumber { get; set; }
        public List<int> RoutePoints { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }
    }
}