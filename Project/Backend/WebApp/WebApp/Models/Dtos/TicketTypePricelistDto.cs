using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
    public class TicketTypePricelistDto
    {   
        public int TicketId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Ticket price calculated by service - ticket base price * transport coefficient.
        /// </summary>
        public double Price { get; set; }
    }
}