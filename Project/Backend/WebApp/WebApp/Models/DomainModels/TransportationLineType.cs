using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.DomainModels
{
    public class TransportationLineType
	{
		public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TransportationLine> TransportationLines { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}