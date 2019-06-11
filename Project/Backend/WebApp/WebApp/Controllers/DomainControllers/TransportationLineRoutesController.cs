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
        public IEnumerable<TransportationLineRoute> GetTransportationLineRoutes()
        {
            return unitOfWork.TransportationLineRouteRepository.GetAll();
        }

        // GET: api/TransportationLineRoutes/5
        [ResponseType(typeof(TransportationLineRoute))]
        public IHttpActionResult GetTransportationLineRoute(int id)
        {
            TransportationLineRoute transportationLineRoute = unitOfWork.TransportationLineRouteRepository.Get(id);
            if (transportationLineRoute == null)
            {
                return NotFound();
            }

            return Ok(transportationLineRoute);
        }

        // PUT: api/TransportationLineRoutes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransportationLineRoute(int id, TransportationLineRoute transportationLineRoute)
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
        [ResponseType(typeof(TransportationLineRoute))]
        public IHttpActionResult PostTransportationLineRoute(TransportationLineRoute transportationLineRoute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.TransportationLineRouteRepository.Add(transportationLineRoute);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = transportationLineRoute.Id }, transportationLineRoute);
        }

        // DELETE: api/TransportationLineRoutes/5
        [ResponseType(typeof(TransportationLineRoute))]
        public IHttpActionResult DeleteTransportationLineRoute(int id)
        {
            TransportationLineRoute transportationLineRoute = unitOfWork.TransportationLineRouteRepository.Get(id);
            if (transportationLineRoute == null)
            {
                return NotFound();
            }

            unitOfWork.TransportationLineRouteRepository.Remove(transportationLineRoute);
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
            return unitOfWork.TransportationLineRouteRepository.Get(id) != null;
        }
    }
}