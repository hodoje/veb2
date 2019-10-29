using AutoMapper;
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
using WebApp.Models.Dtos;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers.DomainControllers
{
    public class TransportationLineTypesController : ApiController
    {
		private IUnitOfWork unitOfWork;
		private IMapper mapper;

		public TransportationLineTypesController()
		{

		}

		public TransportationLineTypesController(IUnitOfWork unitOfWork, IMapper immaper)
		{
			this.unitOfWork = unitOfWork;
			mapper = immaper;
		}

        // GET: api/TransportationLineTypes
        public IEnumerable<TransportationLineTypeDto> GetTransportationLineTypes()
        {
			return mapper.Map<List<TransportationLineType>, List<TransportationLineTypeDto>>(unitOfWork.TransportationLineTypeRepository.GetAll().ToList());
        }

        // GET: api/TransportationLineTypes/5
        [ResponseType(typeof(TransportationLineType))]
        public IHttpActionResult GetTransportationLineType(int id)
        {
            TransportationLineType TransportationLineType = unitOfWork.TransportationLineTypeRepository.Get(id);
            if (TransportationLineType == null)
            {
                return NotFound();
            }

            return Ok(TransportationLineType);
        }

        // PUT: api/TransportationLineTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransportationLineType(int id, TransportationLineType TransportationLineType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != TransportationLineType.Id)
            {
                return BadRequest();
            }

			unitOfWork.TransportationLineTypeRepository.Update(TransportationLineType);

			try
            {
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportationLineTypeExists(id))
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

        // POST: api/TransportationLineTypes
        [ResponseType(typeof(TransportationLineType))]
        public IHttpActionResult PostTransportationLineType(TransportationLineType TransportationLineType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.TransportationLineTypeRepository.Add(TransportationLineType);

            try
            {
				unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                if (TransportationLineTypeExists(TransportationLineType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = TransportationLineType.Id }, TransportationLineType);
        }

        // DELETE: api/TransportationLineTypes/5
        [ResponseType(typeof(TransportationLineType))]
        public IHttpActionResult DeleteTransportationLineType(int id)
        {
            TransportationLineType TransportationLineType = unitOfWork.TransportationLineTypeRepository.Get(id);
            if (TransportationLineType == null)
            {
                return NotFound();
            }

			unitOfWork.TransportationLineTypeRepository.Remove(TransportationLineType);
			unitOfWork.Complete();

            return Ok(TransportationLineType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransportationLineTypeExists(int id)
        {
			return unitOfWork.TransportationLineTypeRepository.Get(id) != null;
        }
    }
}