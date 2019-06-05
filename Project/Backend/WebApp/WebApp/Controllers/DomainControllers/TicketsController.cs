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
using WebApp.Models.Dtos;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers.DomainControllers
{
    public class TicketsController : ApiController
    {
        private ApplicationUserManager userManager;
		private IUnitOfWork unitOfWork;
        private ITicketBusinessComponent ticketBusinessComponent;

		public TicketsController(IUnitOfWork uow, ITicketBusinessComponent ticketBusinessComponent)
		{
			unitOfWork = uow;
            this.ticketBusinessComponent = ticketBusinessComponent;
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

        [Route("api/Tickets/BuyTicket")]
        [HttpPost]
        public async Task<IHttpActionResult> BuyTicket(TicketDto id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                string userName = ((ClaimsIdentity)(Thread.CurrentPrincipal.Identity)).Name;
                ApplicationUser user = await UserManager.FindByNameAsync(userName);

                if (ticketBusinessComponent.BuyTicket(unitOfWork, user, id.TicketTypeId, true))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
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
    }
}