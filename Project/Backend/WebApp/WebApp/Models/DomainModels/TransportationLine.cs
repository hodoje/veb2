using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
			TransportationLineRoutePoints = new HashSet<TransportationLineRoutePoint>();
		}

		public int Id { get; set; }
		public int LineNum { get; set; }
		public ICollection<TransportationLineRoutePoint> TransportationLineRoutePoints { get; set; }
		public ICollection<Schedule> Schedules { get; set; }
		public int TransportationLineTypeId { get; set; }
		public virtual TransportationLineType TransportationLineType { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}