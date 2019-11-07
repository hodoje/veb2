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
    public class TransportationLineRoutesController : ApiController
    {
        private IUnitOfWork unitOfWork;

        public TransportationLineRoutesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/TransportationLineRoutes
        public IEnumerable<TransportationLineRoutePoint> GetTransportationLineRoutes()
        {
            return unitOfWork.TransportationLineRoutePointsRepository.GetAll();
        }

        // GET: api/TransportationLineRoutes/5
        [ResponseType(typeof(TransportationLineRoutePoint))]
        public IHttpActionResult GetTransportationLineRoute(int id)
        {
			TransportationLineRoutePoint transportationLineRoute = unitOfWork.TransportationLineRoutePointsRepository.Get(id);
            if (transportationLineRoute == null)
            {
                return NotFound();
            }

            return Ok(transportationLineRoute);
        }

        // PUT: api/TransportationLineRoutes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransportationLineRoute(int id, TransportationLineRoutePoint transportationLineRoute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transportationLineRoute.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportationLineRouteExists(id))
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

        // POST: api/TransportationLineRoutes
        [ResponseType(typeof(TransportationLineRoutePoint))]
        public IHttpActionResult PostTransportationLineRoute(TransportationLineRoutePoint transportationLineRoute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.TransportationLineRoutePointsRepository.Add(transportationLineRoute);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = transportationLineRoute.Id }, transportationLineRoute);
        }

        // DELETE: api/TransportationLineRoutes/5
        [ResponseType(typeof(TransportationLineRoutePoint))]
        public IHttpActionResult DeleteTransportationLineRoute(int id)
        {
			TransportationLineRoutePoint transportationLineRoute = unitOfWork.TransportationLineRoutePointsRepository.Get(id);
            if (transportationLineRoute == null)
            {
                return NotFound();
            }

            unitOfWork.TransportationLineRoutePointsRepository.Remove(transportationLineRoute);
            unitOfWork.Complete();

            return Ok(transportationLineRoute);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransportationLineRouteExists(int id)
        {
            return unitOfWork.TransportationLineRoutePointsRepository.Get(id) != null;
        }
    }
}