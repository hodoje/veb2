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
    public class TransporationLineTypesController : ApiController
    {
		private IUnitOfWork unitOfWork;

		public TransporationLineTypesController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

        // GET: api/TransporationLineTypes
        public IEnumerable<TransporationLineType> GetTransporationLineTypes()
        {
			return unitOfWork.TransporationLineTypeRepository.GetAll();
        }

        // GET: api/TransporationLineTypes/5
        [ResponseType(typeof(TransporationLineType))]
        public IHttpActionResult GetTransporationLineType(string id)
        {
            TransporationLineType transporationLineType = unitOfWork.TransporationLineTypeRepository.Get(id);
            if (transporationLineType == null)
            {
                return NotFound();
            }

            return Ok(transporationLineType);
        }

        // PUT: api/TransporationLineTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransporationLineType(string id, TransporationLineType transporationLineType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transporationLineType.Id)
            {
                return BadRequest();
            }

			unitOfWork.TransporationLineTypeRepository.Update(transporationLineType);

			try
            {
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransporationLineTypeExists(id))
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

        // POST: api/TransporationLineTypes
        [ResponseType(typeof(TransporationLineType))]
        public IHttpActionResult PostTransporationLineType(TransporationLineType transporationLineType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.TransporationLineTypeRepository.Add(transporationLineType);

            try
            {
				unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                if (TransporationLineTypeExists(transporationLineType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = transporationLineType.Id }, transporationLineType);
        }

        // DELETE: api/TransporationLineTypes/5
        [ResponseType(typeof(TransporationLineType))]
        public IHttpActionResult DeleteTransporationLineType(string id)
        {
            TransporationLineType transporationLineType = unitOfWork.TransporationLineTypeRepository.Get(id);
            if (transporationLineType == null)
            {
                return NotFound();
            }

			unitOfWork.TransporationLineTypeRepository.Remove(transporationLineType);
			unitOfWork.Complete();

            return Ok(transporationLineType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransporationLineTypeExists(string id)
        {
			return unitOfWork.TransporationLineTypeRepository.Get(id) != null;
        }
    }
}