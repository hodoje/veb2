using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.BusinessComponents.NotificationHub
{
    public class VehicleModel
    {
        public VehicleModel(int lineNumber, CurrentPosition positions)
        {
			CurrentPosition = positions;
			LineNumber = lineNumber;
        }
        public int LineNumber { get; set; }
        public CurrentPosition CurrentPosition { get; set; }
    }
}