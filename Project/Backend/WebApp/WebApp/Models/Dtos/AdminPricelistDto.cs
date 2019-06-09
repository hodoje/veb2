using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
	public class AdminPricelistDto
	{
		public DateTime FromDate { get; set; }
		public double Hourly { get; set; }
		public double Daily { get; set; }
		public double Monthly { get; set; }
		public double Yearly { get; set; }
	}
}