using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApp.Models;
using WebApp.Models.DomainModels;
using WebApp.Models.DomainModels.Benefits;
using WebApp.Models.Dtos;
using WebApp.Models.Enumerations;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public class TicketBusinessComponent : ITicketBusinessComponent
    {
        private int BuyTicket(IUnitOfWork unitOfWork, ApplicationUser user, int ticketTypeId)
        {
            TicketType ticketType = unitOfWork.TicketTypeRepository.Get(ticketTypeId);

            if (ticketType == null)
            {
                return -1;
            }

            int plIndex = unitOfWork.PricelistRepository.GetActivePricelist().Id;
            TicketTypePricelist currentPLTT = unitOfWork.TicketTypePricelistRepository.Find(pltt => pltt.PricelistId == plIndex).FirstOrDefault();
            Ticket boughtTicket = new Ticket(ticketType.Name) { PurchaseDate = DateTime.Now };

            List<Ticket> tickets = unitOfWork.TicketRepository.GetAll().ToList();

            try
            {
                unitOfWork.TicketRepository.Add(boughtTicket);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            boughtTicket.ApplicationUserId = user?.Id;
            boughtTicket.TicketTypePricelistId = currentPLTT.Id;

            try
            {
                unitOfWork.Complete();
            }
            catch(Exception e)
            {
                unitOfWork.TicketRepository.Remove(unitOfWork.TicketRepository.Get(boughtTicket.Id));
                throw;
            }

            return boughtTicket.Id;
        }

		public async Task<IEnumerable<TicketTypePricelistDto>> GetTicketPriceForUser(ApplicationUserManager userManager, IUnitOfWork unitOfWork)
		{
			string userName = ((ClaimsIdentity)(Thread.CurrentPrincipal.Identity)).Name;
			ApplicationUser user = await userManager.FindByNameAsync(userName);

			double discountCoefficient = user == null ? 1 : user.GetDiscountCoefficient();
			bool userConfirmed = user != null && user.RegistrationStatus == RegistrationStatus.Accepted;

			return ListTicketPricesForUser(unitOfWork, discountCoefficient, userConfirmed);
		}

		public IEnumerable<TicketTypePricelistDto> ListAllTicketPrices(IUnitOfWork unitOfWork)
        {
            try
            {
                Pricelist currentPriceList = unitOfWork.PricelistRepository.GetActivePricelist();

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

        private IEnumerable<TicketTypePricelistDto> ListTicketPricesForUser(IUnitOfWork unitOfWork, double discountCoefficient, bool userConfirmed)
        {
            try
            {
                Pricelist currentPriceList = unitOfWork.PricelistRepository.GetActivePricelist();

                List<TicketTypePricelist> pltts = unitOfWork.TicketTypePricelistRepository.GetAll().Where(x => x.PricelistId == currentPriceList.Id).ToList();

                List<TicketTypePricelistDto> tickets = new List<TicketTypePricelistDto>();

                if (userConfirmed)
                {
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
                }
                else
                {
                    TicketTypePricelist plttHourly = pltts.First(x => x.TicketType.Name.Equals("Hourly"));

                    tickets.Add(new TicketTypePricelistDto()
                    {
                        Price = plttHourly.BasePrice,
                        Name = plttHourly.TicketType.Name,
                        TicketId = plttHourly.TicketType.Id
                    });
                }

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

				return (ticket != null && ticket.ExpirationDate > DateTime.Now);
			}
			catch
			{
				throw;
			}
		}

		public async Task<HttpStatusCode> TicketPurchase(ApplicationUserManager userManager, IUnitOfWork unitOfWork, TicketDto ticketDto, IEmailSender emailSender)
		{
			string userName = ((ClaimsIdentity)(Thread.CurrentPrincipal.Identity)).Name;
			ApplicationUser user = await userManager.FindByNameAsync(userName);

			int boughtTicketId;
			if ((boughtTicketId = BuyTicket(unitOfWork, user, ticketDto.TicketTypeId)) > 0)
			{
				if (!String.IsNullOrEmpty(ticketDto.Email))
				{
					TicketType ticketType = unitOfWork.TicketTypeRepository.Find(tt => tt.Id == ticketDto.TicketTypeId).FirstOrDefault();
					string subject = $"NS - Public Transport: {ticketType.Name} ticket bought.";
					string body = $"Your {ticketType.Name.ToLower()} ticket id is: #{boughtTicketId}.";
					if (emailSender.SendMail(subject, body, ticketDto.Email))
					{
						return HttpStatusCode.OK;
					}
					else
					{
						unitOfWork.TicketRepository.Remove(unitOfWork.TicketRepository.Get(boughtTicketId));
						unitOfWork.Complete();

						return HttpStatusCode.InternalServerError;
					}
				}
				else
				{
					return HttpStatusCode.BadRequest;
				}

				if (user == null)
				{
					return HttpStatusCode.BadRequest;
				}
			}
			else
			{
				return HttpStatusCode.BadRequest;
			}
		}
	}
}