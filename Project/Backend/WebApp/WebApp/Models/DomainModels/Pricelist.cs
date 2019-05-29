using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class Pricelist
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TicketTypePricelistId { get; set; }
        public virtual TicketTypePricelist TicketTypePricelist { get; set; }
    }
}