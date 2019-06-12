using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class TicketTypePricelist
    {
        public TicketTypePricelist()
        {

        }

        public TicketTypePricelist(TicketType ticketType, Pricelist priceList)
        {
            TicketType = ticketType;
            Pricelist = priceList;
        }

        public TicketTypePricelist(int ticketTypeId, int priceListId)
        {
            TicketTypeId = ticketTypeId;
            PricelistId = priceListId;
        }

        public int Id { get; set; }
        public double BasePrice { get; set; }
        public int TicketTypeId { get; set; }
        public virtual TicketType TicketType { get; set; }
        public int PricelistId { get; set; }
        public virtual Pricelist Pricelist { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}