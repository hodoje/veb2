using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;

namespace WebApp.Models.Dtos
{
    /// <summary>
    /// It is used for representing each TransporationLineRoute wrapped with Station.
    /// </summary>
    public class RoutePointDto
    {
        public Station Station { get; set; }
        public int SequenceNumber { get; set; }
    }
}