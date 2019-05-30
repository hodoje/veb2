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
    public class BenefitsController : ApiController
    {
		private IUnitOfWork unitOfWork;

		public BenefitsController(IUnitOfWork uow)
		{
			unitOfWork = uow;
		}

        // GET: api/Benefits
        public IEnumerable<Benefit> GetBenefits()
        {
			return unitOfWork.BenefitRepository.GetAll();
        }

        // GET: api/Benefits/5
        [ResponseType(typeof(Benefit))]
        public IHttpActionResult GetBenefit(int id)
        {
			Benefit benefit = unitOfWork.BenefitRepository.Get(id);
            if (benefit == null)
            {
                return NotFound();
            }

            return Ok(benefit);
        }

        // PUT: api/Benefits/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBenefit(int id, Benefit benefit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != benefit.Id)
            {
                return BadRequest();
            }

            try
            {
				unitOfWork.BenefitRepository.Update(benefit);
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BenefitExists(id))
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

        // POST: api/Benefits
        [ResponseType(typeof(Benefit))]
        public IHttpActionResult PostBenefit(Benefit benefit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.BenefitRepository.Add(benefit);
			unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = benefit.Id }, benefit);
        }

        // DELETE: api/Benefits/5
        [ResponseType(typeof(Benefit))]
        public IHttpActionResult DeleteBenefit(int id)
        {
			Benefit benefit = unitOfWork.BenefitRepository.Get(id);
            if (benefit == null)
            {
                return NotFound();
            }

			unitOfWork.BenefitRepository.Remove(benefit);
			unitOfWork.Complete();

            return Ok(benefit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BenefitExists(int id)
        {
            return unitOfWork.BenefitRepository.Get(id) != null;
        }
    }
}