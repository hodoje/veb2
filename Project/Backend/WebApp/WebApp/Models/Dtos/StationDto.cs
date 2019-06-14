using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
    public class StationDto
    {
		public int Id { get; set; }
		public string Name { get; set; }
		// x coord
		public double Longitude { get; set; }
		// y coord
		public double Latitude { get; set; }
	}
}