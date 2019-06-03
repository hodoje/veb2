using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CardDuration { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<TicketTypePricelist> TicketTypePricelists { get; set; }
    }
}