using AutoMapper;
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
        private ITransporationLineComponent transporationLineComponent;

		public TransportationLinesController(IUnitOfWork uow, IMapper imapper, ITransporationLineComponent transporationLineComponent)
		{
			unitOfWork = uow;
			mapper = imapper;
            this.transporationLineComponent = transporationLineComponent;
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
        public IHttpActionResult GetPlanForTransporationLine(int lineNumber)
        {
            try
            {
                TransporationLinePlanDto planDto = transporationLineComponent.GetTransporationLinePlan(unitOfWork, lineNumber);

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
        public IHttpActionResult GetPlansForTransporationLine()
        {
            try
            {
                List<int> transporationLines = unitOfWork.TransportationLineRepository.GetAll().Select(x => x.LineNum).ToList();
                List<TransporationLinePlanDto> plans = new List<TransporationLinePlanDto>(transporationLines.Count);

                foreach (int lineNumber in transporationLines)
                {
                    plans.Add(transporationLineComponent.GetTransporationLinePlan(unitOfWork, lineNumber));
                }


                return Ok(plans);
            }
            catch
            {
                return BadRequest();
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