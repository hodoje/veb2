using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class Pricelist
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public ICollection<TicketTypePricelist> TicketTypePricelists { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}