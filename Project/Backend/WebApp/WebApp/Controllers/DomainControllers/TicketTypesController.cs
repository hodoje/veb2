using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models.DomainModels;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers.DomainControllers
{
    public class TicketTypesController : ApiController
    {
		private IUnitOfWork unitOfWork;

		public TicketTypesController(IUnitOfWork uow)
		{
			unitOfWork = uow;
		}

		// GET: api/TicketTypes
		public IEnumerable<TicketType> GetTicketTypes()
        {
			return unitOfWork.TicketTypeRepository.GetAll();
        }

        // GET: api/TicketTypes/5
        [ResponseType(typeof(TicketType))]
        public IHttpActionResult GetTicketType(int id)
        {
			TicketType ticketType = unitOfWork.TicketTypeRepository.Get(id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return Ok(ticketType);
        }

        // PUT: api/TicketTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTicketType(int id, TicketType ticketType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketType.Id)
            {
                return BadRequest();
            }

            try
            {
				unitOfWork.TicketTypeRepository.Update(ticketType);
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketTypeExists(id))
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

        // POST: api/TicketTypes
        [ResponseType(typeof(TicketType))]
        public IHttpActionResult PostTicketType(TicketType ticketType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.TicketTypeRepository.Add(ticketType);
			unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = ticketType.Id }, ticketType);
        }

        // DELETE: api/TicketTypes/5
        [ResponseType(typeof(TicketType))]
        public IHttpActionResult DeleteTicketType(int id)
        {
            TicketType ticketType = unitOfWork.TicketTypeRepository.Get(id);
            if (ticketType == null)
            {
                return NotFound();
            }

			unitOfWork.TicketTypeRepository.Remove(ticketType);
			unitOfWork.Complete();

            return Ok(ticketType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketTypeExists(int id)
        {
            return unitOfWork.TicketTypeRepository.Get(id) != null;
        }
    }
}