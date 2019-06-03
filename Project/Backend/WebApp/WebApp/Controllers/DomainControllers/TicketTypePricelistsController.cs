using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Models.DomainModels;
using WebApp.Models.DomainModels.Benefits;
using WebApp.Models.Dtos;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers.DomainControllers
{
    public class TicketTypePricelistsController : ApiController
    {
        private readonly string authentificationType = "JWT";
		private IUnitOfWork unitOfWork;
        private ApplicationUserManager userManager;

		public TicketTypePricelistsController(IUnitOfWork uow)
		{
			unitOfWork = uow;
		}

        public ApplicationUserManager UserManager
        {
            get { return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { userManager = value; }
        }

        // GET: api/TicketTypePricelists
        public async Task<IHttpActionResult> GetTicketTypePricelists()
        {
            try
            {
                string userName = ((ClaimsIdentity)(Thread.CurrentPrincipal.Identity)).Name;
                ApplicationUser user = await UserManager.FindByNameAsync(userName);

                Pricelist currentPriceList = unitOfWork.PricelistRepository.Find(x => x.FromDate <= DateTime.Now && x.ToDate >= DateTime.Now).FirstOrDefault();

                List<TicketTypePricelist> pltts = unitOfWork.TicketTypePricelistRepository.GetAll().Where(x => x.PricelistId == currentPriceList.Id).ToList();

                List<TicketTypePricelistDto> tickets = new List<TicketTypePricelistDto>();

                double discoutnCoefficient = user == null ? 1 : user.GetDiscountCoefficient();

                pltts.ForEach(pltt =>
                {
                    TicketTypePricelistDto ticket = new TicketTypePricelistDto()
                    {
                        Price = pltt.BasePrice * discoutnCoefficient,
                        Name = pltt.TicketType.Name,
                        TicketId = pltt.TicketType.Id
                    };

                    tickets.Add(ticket);
                });

                return Ok(tickets);
            }
            catch(NullReferenceException nre)
            {
                // log exception 
                return BadRequest("Service is in invalid state.");
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        // GET: api/TicketTypePricelists/5
        [ResponseType(typeof(TicketTypePricelist))]
        public IHttpActionResult GetTicketTypePricelist(int id)
        {
			TicketTypePricelist ticketTypePricelist = unitOfWork.TicketTypePricelistRepository.Get(id);
            if (ticketTypePricelist == null)
            {
                return NotFound();
            }

            return Ok(ticketTypePricelist);
        }

        // PUT: api/TicketTypePricelists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTicketTypePricelist(int id, TicketTypePricelist ticketTypePricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketTypePricelist.Id)
            {
                return BadRequest();
            }

            try
            {
				unitOfWork.TicketTypePricelistRepository.Update(ticketTypePricelist);
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketTypePricelistExists(id))
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

        // POST: api/TicketTypePricelists
        [ResponseType(typeof(TicketTypePricelist))]
        public IHttpActionResult PostTicketTypePricelist(TicketTypePricelist ticketTypePricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.TicketTypePricelistRepository.Add(ticketTypePricelist);
			unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = ticketTypePricelist.Id }, ticketTypePricelist);
        }

        // DELETE: api/TicketTypePricelists/5
        [ResponseType(typeof(TicketTypePricelist))]
        public IHttpActionResult DeleteTicketTypePricelist(int id)
        {
            TicketTypePricelist ticketTypePricelist = unitOfWork.TicketTypePricelistRepository.Get(id);
            if (ticketTypePricelist == null)
            {
                return NotFound();
            }

			unitOfWork.TicketTypePricelistRepository.Remove(ticketTypePricelist);
			unitOfWork.Complete();

            return Ok(ticketTypePricelist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketTypePricelistExists(int id)
        {
            return unitOfWork.TicketTypePricelistRepository.Get(id) != null;
        }
    }
}