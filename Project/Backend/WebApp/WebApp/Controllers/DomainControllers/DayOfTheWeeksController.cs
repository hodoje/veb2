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
    public class DayOfTheWeeksController : ApiController
    {
		private IUnitOfWork unitOfWork;

		public DayOfTheWeeksController(IUnitOfWork uow)
		{
			unitOfWork = uow;
		}

		// GET: api/DayOfTheWeeks
		public IEnumerable<DayOfTheWeek> GetDayOfTheWeeks()
        {
			return unitOfWork.DayOfTheWeekRepository.GetAll();
        }

        // GET: api/DayOfTheWeeks/5
        [ResponseType(typeof(DayOfTheWeek))]
        public IHttpActionResult GetDayOfTheWeek(int id)
        {
			DayOfTheWeek dayOfTheWeek = unitOfWork.DayOfTheWeekRepository.Get(id);
            if (dayOfTheWeek == null)
            {
                return NotFound();
            }

            return Ok(dayOfTheWeek);
        }

        // PUT: api/DayOfTheWeeks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDayOfTheWeek(int id, DayOfTheWeek dayOfTheWeek)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dayOfTheWeek.Id)
            {
                return BadRequest();
            }

            try
            {
				unitOfWork.DayOfTheWeekRepository.Update(dayOfTheWeek);
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DayOfTheWeekExists(id))
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

        // POST: api/DayOfTheWeeks
        [ResponseType(typeof(DayOfTheWeek))]
        public IHttpActionResult PostDayOfTheWeek(DayOfTheWeek dayOfTheWeek)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.DayOfTheWeekRepository.Add(dayOfTheWeek);
			unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = dayOfTheWeek.Id }, dayOfTheWeek);
        }

        // DELETE: api/DayOfTheWeeks/5
        [ResponseType(typeof(DayOfTheWeek))]
        public IHttpActionResult DeleteDayOfTheWeek(int id)
        {
			DayOfTheWeek dayOfTheWeek = unitOfWork.DayOfTheWeekRepository.Get(id);
            if (dayOfTheWeek == null)
            {
                return NotFound();
            }

			unitOfWork.DayOfTheWeekRepository.Remove(dayOfTheWeek);
			unitOfWork.Complete();

            return Ok(dayOfTheWeek);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DayOfTheWeekExists(int id)
        {
            return unitOfWork.DayOfTheWeekRepository.Get(id) != null;
        }
    }
}