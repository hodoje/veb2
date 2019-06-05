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

        // GET: api/TransporationLineTypes
        public IEnumerable<TransportationLineTypeDto> GetTransporationLineTypes()
        {
			return mapper.Map<List<TransportationLineType>, List<TransportationLineTypeDto>>(unitOfWork.TransportationLineTypeRepository.GetAll().ToList());
        }

        // GET: api/TransporationLineTypes/5
        [ResponseType(typeof(TransportationLineType))]
        public IHttpActionResult GetTransporationLineType(int id)
        {
            TransportationLineType transporationLineType = unitOfWork.TransportationLineTypeRepository.Get(id);
            if (transporationLineType == null)
            {
                return NotFound();
            }

            return Ok(transporationLineType);
        }

        // PUT: api/TransporationLineTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransporationLineType(int id, TransportationLineType transporationLineType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transporationLineType.Id)
            {
                return BadRequest();
            }

			unitOfWork.TransportationLineTypeRepository.Update(transporationLineType);

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
        [ResponseType(typeof(TransportationLineType))]
        public IHttpActionResult PostTransporationLineType(TransportationLineType transporationLineType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.TransportationLineTypeRepository.Add(transporationLineType);

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
        [ResponseType(typeof(TransportationLineType))]
        public IHttpActionResult DeleteTransporationLineType(int id)
        {
            TransportationLineType transporationLineType = unitOfWork.TransportationLineTypeRepository.Get(id);
            if (transporationLineType == null)
            {
                return NotFound();
            }

			unitOfWork.TransportationLineTypeRepository.Remove(transporationLineType);
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

        private bool TransporationLineTypeExists(int id)
        {
			return unitOfWork.TransportationLineTypeRepository.Get(id) != null;
        }
    }
}