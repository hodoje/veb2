using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApp.Models;
using WebApp.Models.DomainModels;
using WebApp.Models.DomainModels.Benefits;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public class TicketBusinessComponent : ITicketBusinessComponent
    {
        public int BuyTicket(IUnitOfWork unitOfWork, ApplicationUser user, int ticketTypeId, bool completeTransaction = false)
        {
            try
            {
                TicketType ticketType = unitOfWork.TicketTypeRepository.Get(ticketTypeId);

                if (ticketType == null)
                {
                    return -1;
                }

                TicketTypePricelist currentPLTT = unitOfWork.TicketTypePricelistRepository.Find(pltt => (pltt.Pricelist.FromDate <= DateTime.Now && pltt.Pricelist.ToDate >= DateTime.Now)).FirstOrDefault();
                Ticket boughtTicket = new Ticket(ticketType.Name) { ApplicationUser = user, PurchaseDate = DateTime.Now, TicketTypePricelist = currentPLTT };

				unitOfWork.TicketRepository.Add(boughtTicket);

                if (completeTransaction == true)
                {
                    unitOfWork.Complete();
                }

                return boughtTicket.Id;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<TicketTypePricelistDto> ListAllTicketPrices(IUnitOfWork unitOfWork)
        {
            try
            {
                Pricelist currentPriceList = unitOfWork.PricelistRepository.Find(x => x.FromDate <= DateTime.Now && x.ToDate >= DateTime.Now).FirstOrDefault();

                List<TicketTypePricelist> pltts = unitOfWork.TicketTypePricelistRepository.GetAll().Where(x => x.PricelistId == currentPriceList.Id).ToList();

                List<TicketTypePricelistDto> tickets = new List<TicketTypePricelistDto>();

                List<UserType> userTypes = unitOfWork.UserTypeRepository.GetAll().ToList();

                foreach (UserType userType in userTypes)
                {
                    TransportDiscountBenefit benefit = userType.Benefits.First(x => x.GetType() == typeof(TransportDiscountBenefit)) as TransportDiscountBenefit;
                    pltts.ForEach(pltt =>
                    {
                        TicketTypePricelistDto ticket = new TicketTypePricelistDto()
                        {
                            Price = pltt.BasePrice * (benefit != null ? benefit.CoefficientDiscount : 1),
                            Name = pltt.TicketType.Name,
                            TicketId = pltt.TicketType.Id,
                            UserType = userType.Name
                        };

                        tickets.Add(ticket);
                    });
                }

                return tickets;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<TicketTypePricelistDto> ListTicketPricesForUser(IUnitOfWork unitOfWork, double discountCoefficient)
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

		public bool ValidateTicket(IUnitOfWork unitOfWork, int ticketId)
		{
			try
			{
				Ticket ticket = unitOfWork.TicketRepository.Get(ticketId);

				return (ticket == null || ticket.ExpirationDate > DateTime.Now);
			}
			catch
			{
				throw;
			}
		}
	}
}