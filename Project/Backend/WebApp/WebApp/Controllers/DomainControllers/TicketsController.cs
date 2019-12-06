using AutoMapper;
using Braintree;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.BusinessComponents;
using WebApp.Models;
using WebApp.Models.CustomHttpActionResults;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers.DomainControllers
{
	public class TicketsController : ApiController
	{
		private ApplicationUserManager userManager;
		private IUnitOfWork unitOfWork;
		private IMapper mapper;
		private ITicketBusinessComponent ticketBusinessComponent;
		private IEmailSender emailSender;
		//private IBraintreeConfiguration braintreeConfiguration;

		//public TicketsController(IUnitOfWork uow, IMapper imapper, ITicketBusinessComponent tbc, IEmailSender es, IBraintreeConfiguration bc)
		//{
		//	unitOfWork = uow;
		//	mapper = imapper;
		//	ticketBusinessComponent = tbc;
		//	emailSender = es;
		//	braintreeConfiguration = bc;
		//}

		public TicketsController(IUnitOfWork uow, IMapper imapper, ITicketBusinessComponent tbc, IEmailSender es)
		{
			unitOfWork = uow;
			mapper = imapper;
			ticketBusinessComponent = tbc;
			emailSender = es;
		}

		public ApplicationUserManager UserManager
		{
			get { return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
			set { userManager = value; }
		}

		// GET: api/Tickets
		public IEnumerable<Ticket> GetTickets()
		{
			return unitOfWork.TicketRepository.GetAll();
		}

		// GET: api/Tickets/5
		[ResponseType(typeof(Ticket))]
		public IHttpActionResult GetTicket(int id)
		{
			Ticket ticket = unitOfWork.TicketRepository.Get(id);
			if (ticket == null)
			{
				return NotFound();
			}

			return Ok(ticket);
		}

		// PUT: api/Tickets/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PutTicket(int id, Ticket ticket)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != ticket.Id)
			{
				return BadRequest();
			}

			try
			{
				unitOfWork.TicketRepository.Update(ticket);
				unitOfWork.Complete();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TicketExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// POST: api/Tickets
		[ResponseType(typeof(Ticket))]
		public IHttpActionResult PostTicket(Ticket ticket)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			unitOfWork.TicketRepository.Add(ticket);
			unitOfWork.Complete();

			return CreatedAtRoute("DefaultApi", new { id = ticket.Id }, ticket);
		}

		[Route("api/Tickets/PaypalPurchase")]
		[HttpPost]
		public async Task<IHttpActionResult> PaypalPurchase(TransactionTicketWrapper ttWrapper)
		{
			ApplicationUser user = await UserManager.FindByEmailAsync(ttWrapper.UserEmail);

			try
			{
				// TODO: Test this
				GeneralTransactionDto transactionDto = ttWrapper.Transaction;				
				TicketDto ticketDto = ttWrapper.TicketDto;

				HttpStatusCode purchaseStatus = await ticketBusinessComponent.StorePaypalTransaction(user, unitOfWork, mapper, transactionDto, ticketDto, emailSender);
				switch (purchaseStatus)
				{
					case HttpStatusCode.OK:
						return Ok();
					case HttpStatusCode.InternalServerError:
						return InternalServerError();
					case HttpStatusCode.BadRequest:
						return BadRequest();
					case HttpStatusCode.Forbidden:
						return new ForbiddenActionResult(Request, "This user is banned.");
					default:
						return BadRequest();
				}
			}
			catch (Exception e)
			{
				return InternalServerError();
			}
		}

		//[Route("api/Tickets/BuyTicket")]
		//[HttpPost]
		//public async Task<IHttpActionResult> BuyTicket(TicketDto ticketDto)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}
		//	try
		//	{
		//		HttpStatusCode purchaseStatus = await ticketBusinessComponent.TicketPurchase(UserManager, unitOfWork, ticketDto, emailSender);
		//		switch (purchaseStatus)
		//		{
		//			case HttpStatusCode.OK:
		//				return Ok();
		//			case HttpStatusCode.InternalServerError:
		//				return InternalServerError();
		//			case HttpStatusCode.BadRequest:
		//				return BadRequest();
		//			case HttpStatusCode.Forbidden:
		//				return new ForbiddenActionResult(Request, "This user is banned.");
		//			default:
		//				return Ok();
		//		}
		//	}
		//	catch (Exception e)
		//	{
		//		return InternalServerError();
		//	}
		//}

		//[Route("api/Tickets/CreatePurchase")]
		//[HttpPost]
		//public async Task<IHttpActionResult> CreatePurchase()
		//{
		//	string nonceFromTheClient = "";
		//	try
		//	{
		//		dynamic createRequestJson = await Request.Content.ReadAsAsync<JObject>();
		//		nonceFromTheClient = createRequestJson.paymentMethodNonce;
		//	}
		//	catch(Exception e)
		//	{

		//	}
		//	// Use payment method nonce here
		//	try
		//	{
		//		HttpStatusCode purchaseStatus = await ticketBusinessComponent.CreatePurchase(braintreeConfiguration, nonceFromTheClient);
		//		switch (purchaseStatus)
		//		{
		//			case HttpStatusCode.OK:
		//				return Ok();
		//			case HttpStatusCode.InternalServerError:
		//				return InternalServerError();
		//			case HttpStatusCode.BadRequest:
		//				return BadRequest();
		//			case HttpStatusCode.Forbidden:
		//				return new ForbiddenActionResult(Request, "This user is banned.");
		//			default:
		//				return Ok();
		//		}
		//	}
		//	catch (Exception e)
		//	{
		//		return InternalServerError();
		//	}			
		//}

		[Route("api/Tickets/ValidateTicket")]
		[Authorize(Roles = "Controller")]
		[HttpPost]
		public async Task<IHttpActionResult> ValidateTicket(TicketDto ticketDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				bool validationResult = await ticketBusinessComponent.ValidateTicket(UserManager, unitOfWork, ticketDto.TicketTypeId);

				if (validationResult)
				{
					return Ok(new
					{
						isValid = "valid"
					});
				}
				else
				{
					return Ok(new
					{
						isValid = "notValid"
					});
				}
			}
			catch (Exception e)
			{
				return InternalServerError();
			}
		}

		// DELETE: api/Tickets/5
		[ResponseType(typeof(Ticket))]
		public IHttpActionResult DeleteTicket(int id)
		{
			Ticket ticket = unitOfWork.TicketRepository.Get(id);
			if (ticket == null)
			{
				return NotFound();
			}

			unitOfWork.TicketRepository.Remove(ticket);
			unitOfWork.Complete();

			return Ok(ticket);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				unitOfWork.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool TicketExists(int id)
		{
			return unitOfWork.TicketRepository.Get(id) != null;
		}

		//#region BrainTree Checkout
		//public IBraintreeConfiguration config = new BraintreeConfiguration();
		//public static readonly TransactionStatus[] transactionSuccessStatuses = {
		//																			TransactionStatus.AUTHORIZED,
		//																			TransactionStatus.AUTHORIZING,
		//																			TransactionStatus.SETTLED,
		//																			TransactionStatus.SETTLING,
		//																			TransactionStatus.SETTLEMENT_CONFIRMED,
		//																			TransactionStatus.SETTLEMENT_PENDING,
		//																			TransactionStatus.SUBMITTED_FOR_SETTLEMENT
		//																		};

		//public IHttpActionResult New()
		//{
		//	var gateway = config.GetGateway();
		//	var clientToken = gateway.ClientToken.Generate();
		//	ViewBag.ClientToken = clientToken;
		//	return View();
		//}

		//public ActionResult Create()
		//{
		//	var gateway = config.GetGateway();
		//	Decimal amount;

		//	try
		//	{
		//		amount = Convert.ToDecimal(Request["amount"]);
		//	}
		//	catch (FormatException e)
		//	{
		//		TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
		//		return RedirectToAction("New");
		//	}

		//	var nonce = Request["payment_method_nonce"];
		//	var request = new TransactionRequest
		//	{
		//		Amount = amount,
		//		PaymentMethodNonce = nonce,
		//		Options = new TransactionOptionsRequest
		//		{
		//			SubmitForSettlement = true
		//		}
		//	};

		//	Result<Transaction> result = gateway.Transaction.Sale(request);
		//	if (result.IsSuccess())
		//	{
		//		Transaction transaction = result.Target;
		//		return RedirectToAction("Show", new { id = transaction.Id });
		//	}
		//	else if (result.Transaction != null)
		//	{
		//		return RedirectToAction("Show", new { id = result.Transaction.Id });
		//	}
		//	else
		//	{
		//		string errorMessages = "";
		//		foreach (ValidationError error in result.Errors.DeepAll())
		//		{
		//			errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
		//		}
		//		TempData["Flash"] = errorMessages;
		//		return RedirectToAction("New");
		//	}

		//}

		//public ActionResult Show(String id)
		//{
		//	var gateway = config.GetGateway();
		//	Transaction transaction = gateway.Transaction.Find(id);

		//	if (transactionSuccessStatuses.Contains(transaction.Status))
		//	{
		//		TempData["header"] = "Sweet Success!";
		//		TempData["icon"] = "success";
		//		TempData["message"] = "Your test transaction has been successfully processed. See the Braintree API response and try again.";
		//	}
		//	else
		//	{
		//		TempData["header"] = "Transaction Failed";
		//		TempData["icon"] = "fail";
		//		TempData["message"] = "Your test transaction has a status of " + transaction.Status + ". See the Braintree API response and try again.";
		//	};

		//	ViewBag.Transaction = transaction;
		//	return View();
		//}
		//#endregion
	}
}