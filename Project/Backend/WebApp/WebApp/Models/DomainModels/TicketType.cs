using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<TicketTypePricelist> TicketTypePricelists { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}