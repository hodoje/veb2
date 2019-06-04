using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
	/// <summary>
	/// Represents a transportation route.
	/// </summary>
	// Checked
	public class TransportationLine
	{
		public TransportationLine()
		{
			Stations = new HashSet<Station>();
		}

		public int Id { get; set; }
		public int LineNum { get; set; }
		public ICollection<Station> Stations { get; set; }
		public ICollection<Schedule> Schedules { get; set; }
		public int TransporationLineTypeId { get; set; }
		public virtual TransporationLineType TransporationLineType { get; set; }
	}
}