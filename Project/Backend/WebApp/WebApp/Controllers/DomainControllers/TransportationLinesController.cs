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
				// Current idea to update Many-to-Many relationship is to delete all Many-to-Many table entries and add new ones
				TransportationLine tLineToBeUpdated = unitOfWork.TransportationLineRepository.Find(tl => tl.LineNum == updatedLinePlan.LineNumber).FirstOrDefault();
				if(tLineToBeUpdated != null)
				{
					List<TransportationLineRoutePoint> tLineToBeUpdatedRoutePoints = unitOfWork.TransportationLineRoutePointsRepository.
						Find(rp => rp.TransportationLineId == tLineToBeUpdated.Id).ToList();
					unitOfWork.TransportationLineRoutePointsRepository.RemoveRange(tLineToBeUpdatedRoutePoints);
					unitOfWork.Complete();

					List<TransportationLineRoutePoint> tLToBeUpdatedRoutePointsToAddList = new List<TransportationLineRoutePoint>(updatedLinePlan.RoutePoints.Count);
					foreach(var rp in updatedLinePlan.RoutePoints)
					{
						TransportationLineRoutePoint tlrp = new TransportationLineRoutePoint();
						tlrp.TransportationLineId = tLineToBeUpdated.Id;
						tlrp.StationId = rp.Station.Id;
						tLToBeUpdatedRoutePointsToAddList.Add(tlrp);
					}

					unitOfWork.TransportationLineRoutePointsRepository.AddRange(tLToBeUpdatedRoutePointsToAddList);
					unitOfWork.Complete();
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

		// POST api/TransportationLines/AddStationToPlan
		[HttpPost]
		[Route("api/TransportationLines/AddStationToPlan")]
		[Authorize(Roles = "Admin")]
		public IHttpActionResult AddStationToPlan(StationToPlanDto dto)
		{
			try
			{
				List<TransportationLineRoutePoint> routes = unitOfWork.TransportationLineRoutePointsRepository.GetAll().Where(x => x.TransportationLine.LineNum == dto.LineNumber).ToList();

				TransportationLineRoutePoint route = routes.FirstOrDefault(x => x.StationId == dto.StationId);
				TransportationLine tl = unitOfWork.TransportationLineRepository.GetAll().Where(x => x.LineNum == dto.LineNumber).FirstOrDefault();
				if (route == null)
				{
					route = new TransportationLineRoutePoint() { SequenceNo = routes.Max(x => x.SequenceNo) + 1, StationId = dto.StationId, TransportationLineId = tl.Id };
					unitOfWork.TransportationLineRoutePointsRepository.Add(route);
					unitOfWork.Complete();

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

		// GET api/TransportationLines/AddStationToPlan
		[HttpPost]
		[Route("api/TransportationLines/RemoveStationFromPlan")]
		[Authorize(Roles = "Admin")]
		public IHttpActionResult RemoveStationFromPlan(StationToPlanDto dto)
		{
			try
			{
				List<TransportationLineRoutePoint> routes = unitOfWork.TransportationLineRoutePointsRepository.GetAll().Where(x => x.TransportationLine.LineNum == dto.LineNumber).ToList();

				TransportationLineRoutePoint route = routes.FirstOrDefault(x => x.StationId == dto.StationId);

				if (route != null)
				{
					unitOfWork.TransportationLineRoutePointsRepository.Remove(route);

					unitOfWork.Complete();

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