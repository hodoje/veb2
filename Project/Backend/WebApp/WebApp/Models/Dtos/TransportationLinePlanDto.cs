using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
    public class TransportationLinePlanDto
    {
        public int LineNumber { get; set; }
        public List<RoutePointDto> RoutePoints { get; set; }
    }
}