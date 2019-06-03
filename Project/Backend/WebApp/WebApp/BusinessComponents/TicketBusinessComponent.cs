using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApp.Models;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public class TicketBusinessComponent : ITicketBusinessComponent
    {
        public bool BuyTicket(ApplicationUser user, int ticketTypeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TicketTypePricelistDto> ListAvailableTicketsWithPrices(IUnitOfWork unitOfWork, double discountCoefficient)
        {
            try
            {
                Pricelist currentPriceList = unitOfWork.PricelistRepository.Find(x => x.FromDate <= DateTime.Now && x.ToDate >= DateTime.Now).FirstOrDefault();

                List<TicketTypePricelist> pltts = unitOfWork.TicketTypePricelistRepository.GetAll().Where(x => x.PricelistId == currentPriceList.Id).ToList();

                List<TicketTypePricelistDto> tickets = new List<TicketTypePricelistDto>();

                pltts.ForEach(pltt =>
                {
                    TicketTypePricelistDto ticket = new TicketTypePricelistDto()
                    {
                        Price = pltt.BasePrice * discountCoefficient,
                        Name = pltt.TicketType.Name,
                        TicketId = pltt.TicketType.Id
                    };

                    tickets.Add(ticket);
                });

                return tickets;
            }
            catch
            {
                throw;
            }
        }
    }
}