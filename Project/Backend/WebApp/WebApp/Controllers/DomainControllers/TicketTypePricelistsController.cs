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
using WebApp.BusinessComponents;
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
        private ITicketBusinessComponent ticketBusiness;
        private ApplicationUserManager userManager;

		public TicketTypePricelistsController(IUnitOfWork uow, ITicketBusinessComponent ticketBusiness)
		{
			unitOfWork = uow;
            this.ticketBusiness = ticketBusiness;
		}

        public ApplicationUserManager UserManager
        {
            get { return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { userManager = value; }
        }

        // GET: api/TicketTypePricelists
        public async Task<IHttpActionResult> GetTicketTypePricelists()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userName = ((ClaimsIdentity)(Thread.CurrentPrincipal.Identity)).Name;
                ApplicationUser user = await UserManager.FindByNameAsync(userName);

                double discountCoefficient = user == null ? 1 : user.GetDiscountCoefficient();

                IEnumerable<TicketTypePricelistDto> tickets = ticketBusiness.ListAvailableTicketsWithPrices(unitOfWork, discountCoefficient);

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