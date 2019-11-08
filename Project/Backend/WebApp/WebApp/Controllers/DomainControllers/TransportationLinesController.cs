using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.BusinessComponents;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers.DomainControllers
{
    public class TransportationLinesController : ApiController
    {
		private IUnitOfWork unitOfWork;
		private IMapper mapper;
        private ITransportationLineComponent transportationLineComponent;

		public TransportationLinesController(IUnitOfWork uow, IMapper imapper, ITransportationLineComponent transportationLineComponent)
		{
			unitOfWork = uow;
			mapper = imapper;
            this.transportationLineComponent = transportationLineComponent;
		}

		// GET: api/TransportationLines
		public IEnumerable<TransportationLineDto> GetTransportationLines()
        {
			return mapper.Map<List<TransportationLine>, List<TransportationLineDto>>(unitOfWork.TransportationLineRepository.GetAll().ToList());
		}

        // GET: api/TransportationLines/5
        [ResponseType(typeof(TransportationLine))]
        public IHttpActionResult GetTransportationLine(int id)
        {
            TransportationLine transportationLine = unitOfWork.TransportationLineRepository.Get(id);
            if (transportationLine == null)
            {
                return NotFound();
            }

            return Ok(transportationLine);
        }

        // GET: api/TransportationLines/Plan/5
        [Route("api/TransportationLines/Plan")]
        public IHttpActionResult GetPlanForTransportationLine(int lineNumber)
        {
            try
            {
                TransportationLinePlanDto planDto = transportationLineComponent.GetTransportationLinePlan(unitOfWork, lineNumber);

                if (planDto == null)
                {
                    return BadRequest();
                }

                return Ok(planDto);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/TransportationLines/Plans
        [Route("api/TransportationLines/Plans")]
        public IHttpActionResult GetPlansForTransportationLine()
        {
            try
            {
                List<int> transportationLines = unitOfWork.TransportationLineRepository.GetAll().Select(x => x.LineNum).ToList();
                List<TransportationLinePlanDto> plans = new List<TransportationLinePlanDto>(transportationLines.Count);

                foreach (int lineNumber in transportationLines)
                {
                    plans.Add(transportationLineComponent.GetTransportationLinePlan(unitOfWork, lineNumber));
                }


                return Ok(plans);
            }
            catch
            {
                return BadRequest();
            }
        }

		[HttpPost]
		[Route("api/TransportationLines/UpdateTransportationLinePlan")]
		[Authorize(Roles = "Admin")]
		public IHttpActionResult UpdateTransportationLinePlan(TransportationLinePlanDto updatedLinePlan)
		{
			try
			{
				if(transportationLineComponent.UpdateTransportationLinePlan(unitOfWork, updatedLinePlan))
				{
					return Ok();
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception e)
			{
				return InternalServerError();
			}
		}

		// PUT: api/TransportationLines/5
		[ResponseType(typeof(void))]
        public IHttpActionResult PutTransportationLine(int id, TransportationLine transportationLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transportationLine.Id)
            {
                return BadRequest();
            }

            try
            {
				unitOfWork.TransportationLineRepository.Update(transportationLine);
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportationLineExists(id))
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

        // POST: api/TransportationLines
        [ResponseType(typeof(TransportationLine))]
        public IHttpActionResult PostTransportationLine(TransportationLine transportationLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.TransportationLineRepository.Add(transportationLine);
			unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = transportationLine.Id }, transportationLine);
        }

        // DELETE: api/TransportationLines/5
        [ResponseType(typeof(TransportationLine))]
        public IHttpActionResult DeleteTransportationLine(int id)
        {
            TransportationLine transportationLine = unitOfWork.TransportationLineRepository.Get(id);
            if (transportationLine == null)
            {
                return NotFound();
            }

			unitOfWork.TransportationLineRepository.Remove(transportationLine);
			unitOfWork.Complete();

            return Ok(transportationLine);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransportationLineExists(int id)
        {
            return unitOfWork.TransportationLineRepository.Get(id) != null;
        }
    }
}