using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class TicketTypePricelist
    {
        public int Id { get; set; }
        public double BasePrice { get; set; }
        public int TicketTypeId { get; set; }
        public virtual TicketType TicketType { get; set; }
        public int PricelistId { get; set; }
        public virtual Pricelist Pricelist { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}