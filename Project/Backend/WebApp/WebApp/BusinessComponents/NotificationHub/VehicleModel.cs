using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.BusinessComponents.NotificationHub
{
    public class VehicleModel
    {
        public VehicleModel(List<CurrentPosition> positions)
        {
            RoutePoints = new Queue<CurrentPosition>(positions);
        }
        public int LineNumber { get; set; }
        public Queue<CurrentPosition> RoutePoints { get; private set; }

        public CurrentPosition GetNextStop()
        {
            CurrentPosition nextStop = RoutePoints.Dequeue();
            RoutePoints.Enqueue(nextStop);

            return nextStop;
        }
    }
}