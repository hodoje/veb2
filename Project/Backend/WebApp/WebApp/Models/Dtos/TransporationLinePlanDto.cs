using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
    public class TransporationLinePlanDto
    {
        public int LineNumber { get; set; }
        public List<RoutePointDto> Routes { get; set; }
    }
}