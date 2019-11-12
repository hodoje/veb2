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
	// TODO: refactor this whole component
    public class TicketBusinessComponent : ITicketBusinessComponent
    {
		public async Task<IEnumerable<TicketTypePricelistDto>> GetTicketPriceForUser(ApplicationUserManager userManager, IUnitOfWork unitOfWork)
		{
			string username = ((ClaimsIdentity)(Thread.CurrentPrincipal.Identity)).Name;
			ApplicationUser user = await userManager.FindByNameAsync(username);

			if (user != null && user.Banned)
			{
				throw new Exception();
			}

			double discountCoefficient = user == null ? 1 : user.GetDiscountCoefficient();
			bool userConfirmed = user != null && user.RegistrationStatus == RegistrationStatus.Accepted;

			return ListTicketPricesForUser(unitOfWork, discountCoefficient, userConfirmed);
		}

		public IEnumerable<TicketTypePricelistDto> ListAllTicketPrices(ApplicationUserManager userManager, IUnitOfWork unitOfWork)
        {
            try
            {
                Pricelist currentPriceList = unitOfWork.PricelistRepository.GetActivePricelist();

                List<TicketTypePricelist> ttpls = unitOfWork.TicketTypePricelistRepository.GetAll().Where(x => x.PricelistId == currentPriceList.Id).ToList();

                List<TicketTypePricelistDto> tickets = new List<TicketTypePricelistDto>();

                List<UserType> userTypes = unitOfWork.UserTypeRepository.GetAll().ToList();

                foreach (UserType userType in userTypes)
                {
                    TransportDiscountBenefit benefit = userType.Benefits.First(x => x.GetType() == typeof(TransportDiscountBenefit)) as TransportDiscountBenefit;
					ttpls.ForEach(ttpl =>
                    {
                        TicketTypePricelistDto ticket = new TicketTypePricelistDto()
                        {
                            Price = ttpl.BasePrice * (benefit != null ? benefit.CoefficientDiscount : 1),
                            Name = ttpl.TicketType.Name,
                            TicketId = ttpl.TicketType.Id,
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

		public async Task<bool> ValidateTicket(ApplicationUserManager userManager, IUnitOfWork unitOfWork, int ticketId)
		{
			string username = ((ClaimsIdentity)(Thread.CurrentPrincipal.Identity)).Name;
			ApplicationUser user = await userManager.FindByNameAsync(username);

			if (user.Banned)
			{
				throw new Exception();
			}

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
			string username = ((ClaimsIdentity)(Thread.CurrentPrincipal.Identity)).Name;
			ApplicationUser user = await userManager.FindByNameAsync(username);

			if (user != null && user.Banned)
			{
				return HttpStatusCode.Forbidden;
			}

			int boughtTicketId;
			if ((boughtTicketId = BuyTicket(unitOfWork, user, ticketDto.TicketTypeId)) > 0)
			{
				string email = "";

				if(user != null)
				{
					email = user.Email;
				}
				else
				{
					email = ticketDto.Email;
				}

				if (!String.IsNullOrEmpty(email))
				{
					TicketType ticketType = unitOfWork.TicketTypeRepository.Find(tt => tt.Id == ticketDto.TicketTypeId).FirstOrDefault();
					string subject = $"NS - Public Transport: {ticketType.Name} ticket bought.";
					string body = $"Your {ticketType.Name.ToLower()} ticket id is: #{boughtTicketId}.";
					if (emailSender.SendMail(subject, body, email))
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
			}
			else
			{
				return HttpStatusCode.BadRequest;
			}
		}

		private int BuyTicket(IUnitOfWork unitOfWork, ApplicationUser user, int ticketTypeId)
		{
			TicketType ticketType = unitOfWork.TicketTypeRepository.Get(ticketTypeId);

			if (ticketType == null)
			{
				return -1;
			}

			int plIndex = unitOfWork.PricelistRepository.GetActivePricelist().Id;
			TicketTypePricelist currentPLTT = unitOfWork.TicketTypePricelistRepository.Find(ttpl => ttpl.PricelistId == plIndex).FirstOrDefault();
			Ticket boughtTicket = new Ticket(ticketType.Name) { PurchaseDate = DateTime.Now };

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
			catch (Exception e)
			{
				unitOfWork.TicketRepository.Remove(unitOfWork.TicketRepository.Get(boughtTicket.Id));
				throw;
			}

			return boughtTicket.Id;
		}

		private IEnumerable<TicketTypePricelistDto> ListTicketPricesForUser(IUnitOfWork unitOfWork, double discountCoefficient, bool userConfirmed)
        {
            try
            {
                Pricelist currentPriceList = unitOfWork.PricelistRepository.GetActivePricelist();

                List<TicketTypePricelist> ttpls = unitOfWork.TicketTypePricelistRepository.GetAll().Where(x => x.PricelistId == currentPriceList.Id).ToList();

                List<TicketTypePricelistDto> tickets = new List<TicketTypePricelistDto>();

                if (userConfirmed)
                {
					ttpls.ForEach(ttpl =>
                    {
                        TicketTypePricelistDto ticket = new TicketTypePricelistDto()
                        {
                            Price = ttpl.BasePrice * discountCoefficient,
                            Name = ttpl.TicketType.Name,
                            TicketId = ttpl.TicketType.Id
                        };

                        tickets.Add(ticket);
                    });
                }
                else
                {
                    TicketTypePricelist plttHourly = ttpls.First(x => x.TicketType.Name.Equals("Hourly"));

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
	}
}