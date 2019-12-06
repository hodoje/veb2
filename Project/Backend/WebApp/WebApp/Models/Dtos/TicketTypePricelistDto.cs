using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
    public class TicketTypePricelistDto
    {   
        public int TicketTypeId { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        /// <summary>
        /// Ticket price calculated by service - ticket base price * transport coefficient.
        /// </summary>
        public double Price { get; set; }
    }
}