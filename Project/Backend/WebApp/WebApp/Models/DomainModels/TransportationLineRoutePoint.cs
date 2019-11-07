using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    public class TransportationLineRoutePoint
    {
        public int Id { get; set; }
        public int SequenceNo { get; set; }
        public int TransportationLineId { get; set; }
        public virtual TransportationLine TransportationLine { get; set; }
        public int StationId { get; set; }
        public virtual Station Station { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}