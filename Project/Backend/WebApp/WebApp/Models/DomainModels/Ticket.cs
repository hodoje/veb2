using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Models.Enumerations;

namespace WebApp.Models.DomainModels
{
    public class Ticket
    {
        public Ticket()
        {

        }

        public Ticket(string ticketType)
        {
            DateTime currentTime = DateTime.Now;
            switch((TicketTypeEnum)Enum.Parse(typeof(TicketTypeEnum), ticketType))
            {
                case TicketTypeEnum.Hourly:
                    ExpirationDate = currentTime.Add(new TimeSpan(1, 0, 0));
                    break;
                case TicketTypeEnum.Daily:
                    currentTime.AddDays(1);
                    ExpirationDate = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0);
                    break;
                case TicketTypeEnum.Monthly:
                    currentTime.AddMonths(1);
                    ExpirationDate = new DateTime(currentTime.Year, currentTime.Month, 1, 0, 0, 0);
                    break;
                case TicketTypeEnum.Yearly:
                    currentTime.AddYears(1);
                    ExpirationDate = new DateTime(currentTime.Year, 1, 1, 0, 0, 0);
                    break;
            }
        }

        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int? TicketTypePricelistId { get; set; }
        public TicketTypePricelist TicketTypePricelist { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}