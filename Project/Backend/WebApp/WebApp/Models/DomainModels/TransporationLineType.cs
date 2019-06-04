using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
	public class TransporationLineType
	{
		/// <summary>
		/// Represents type of zone.
		/// </summary>
		public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TransportationLine> TransportationLines { get; set; }
	}
}