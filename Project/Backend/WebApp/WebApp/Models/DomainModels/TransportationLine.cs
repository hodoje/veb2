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
            TransportationLineRoutes = new HashSet<TransportationLineRoute>();
		}

		public int Id { get; set; }
		public int LineNum { get; set; }
		public ICollection<TransportationLineRoute> TransportationLineRoutes { get; set; }
		public ICollection<Schedule> Schedules { get; set; }
		public int TransportationLineTypeId { get; set; }
		public virtual TransportationLineType TransportationLineType { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}