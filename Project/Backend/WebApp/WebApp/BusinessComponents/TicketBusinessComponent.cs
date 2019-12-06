using AutoMapper;
using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Unity;
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
		#region Getters
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
                            TicketTypeId = ttpl.TicketType.Id,
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
							TicketTypeId = ttpl.TicketType.Id
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
						TicketTypeId = plttHourly.TicketType.Id
					});
				}

				return tickets;
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region Paypal
		public async Task<HttpStatusCode> StorePaypalTransaction(ApplicationUser user, IUnitOfWork unitOfWork, IMapper mapper, GeneralTransactionDto transactionDto, TicketDto ticketDto, IEmailSender emailSender)
		{
			if (user != null && user.Banned)
			{
				return HttpStatusCode.Forbidden;
			}

			GeneralTransaction transaction = mapper.Map<GeneralTransactionDto, GeneralTransaction>(transactionDto);
			// Store transaction
			try
			{
				if(user == null)
				{
					transaction.UserId = "notregistered@nomail.com";
				}
				else
				{
					transaction.UserId = user.Id;
				}
				
				unitOfWork.TransactionRepository.Add(transaction);
				unitOfWork.Complete();
			}
			catch(Exception e)
			{
				unitOfWork.TransactionRepository.Remove(transaction);
			}
			///////////////////

			// Store bought ticket			
			return await TicketPurchase(user, unitOfWork, ticketDto, emailSender);
			///////////////////
		}
		#endregion

		#region Other Credit Cards
		//public async Task<HttpStatusCode> CreatePurchase(ApplicationUserManager userManager,
		//															 IUnitOfWork unitOfWork,
		//																TicketDto ticketDto,
		//														   IEmailSender emailSender,
		//									 IBraintreeConfiguration braintreeConfiguration,
		//															 string nonceFromTheClient)
		//{
		//	// Get ticket price for user so we can add it to transaction
		//	IEnumerable<TicketTypePricelistDto> userTicketPrices = await GetTicketPriceForUser(userManager, unitOfWork);
		//	TicketTypePricelistDto ttplDto = null;
		//	foreach (var ttpl in userTicketPrices)
		//	{
		//		if (ticketDto.TicketTypeId == ttpl.TicketTypeId)
		//		{
		//			ttplDto = ttpl;
		//			break;
		//		}
		//	}

		//	if (ttplDto != null)
		//	{
		//		return await Task.Run(async () =>
		//		{
		//			var request = new TransactionRequest
		//			{
		//				Amount = (decimal)ttplDto.Price,
		//				PaymentMethodNonce = nonceFromTheClient,
		//				Options = new TransactionOptionsRequest
		//				{
		//					SubmitForSettlement = true
		//				}
		//			};
		//			var braintreeGateway = braintreeConfiguration.GetGateway();
		//			try
		//			{
		//				Result<Transaction> result = braintreeGateway.Transaction.Sale(request);
		//				if (result != null)
		//				{
		//					Transaction targetTransaction = result.Target;
		//					if (targetTransaction != null)
		//					{
		//						Transaction transaction = braintreeGateway.Transaction.Find(targetTransaction.Id);
		//						if (transaction != null)
		//						{
		//							if (transactionSuccessStatuses.Contains(transaction.Status))
		//							{
		//								return await TicketPurchase(userManager, unitOfWork, ticketDto, emailSender);
		//							}
		//						}
		//					}
		//				}
		//				return HttpStatusCode.BadRequest;
		//			}
		//			catch (Exception e)
		//			{
		//				return HttpStatusCode.InternalServerError;
		//			}
		//		});
		//	}
		//	else
		//	{
		//		return HttpStatusCode.BadRequest;
		//	}
		//}
		#endregion

		#region Buy and Validate
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

		private async Task<HttpStatusCode> TicketPurchase(ApplicationUser user, IUnitOfWork unitOfWork, TicketDto ticketDto, IEmailSender emailSender)
		{
			HttpStatusCode response = HttpStatusCode.BadRequest;

			await Task.Run(() =>
			{
				int boughtTicketId;
				if ((boughtTicketId = StoreTicket(unitOfWork, user, ticketDto.TicketTypeId)) > 0)
				{
					string email = user != null ? user.Email : ticketDto.Email;

					if (!String.IsNullOrEmpty(email))
					{
						TicketType ticketType = unitOfWork.TicketTypeRepository.Find(tt => tt.Id == ticketDto.TicketTypeId).FirstOrDefault();

						string subject = $"NS - Public Transport: {ticketType.Name} ticket bought.";
						string body = $"Your {ticketType.Name.ToLower()} ticket id is: #{boughtTicketId}.";

						if (emailSender.SendMail(subject, body, email))
						{
							response = HttpStatusCode.OK;
						}
						else
						{
							unitOfWork.TicketRepository.Remove(unitOfWork.TicketRepository.Get(boughtTicketId));
							unitOfWork.Complete();

							response = HttpStatusCode.BadRequest;
						}
					}
				}
			});

			return response;
		}

		private int StoreTicket(IUnitOfWork unitOfWork, ApplicationUser user, int ticketTypeId)
		{
			TicketType ticketType = unitOfWork.TicketTypeRepository.Get(ticketTypeId);

			if (ticketType == null)
			{
				return -1;
			}

			int plId = unitOfWork.PricelistRepository.GetActivePricelist().Id;
			TicketTypePricelist currentTtpl = unitOfWork.TicketTypePricelistRepository.Find(ttpl => ttpl.PricelistId == plId).FirstOrDefault();
			Ticket boughtTicket = new Ticket(ticketType.Name) { PurchaseDate = DateTime.Now };

			try
			{
				unitOfWork.TicketRepository.Add(boughtTicket);
				unitOfWork.Complete();
			}
			catch(Exception e)
			{
				throw;
			}

			boughtTicket.ApplicationUserId = user?.Id;
			boughtTicket.TicketTypePricelistId = currentTtpl?.Id;

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
		#endregion
	}
}