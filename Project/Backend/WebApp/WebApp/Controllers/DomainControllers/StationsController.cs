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
    public class StationsController : ApiController
    {
		private IUnitOfWork unitOfWork;
		private IMapper mapper;

		public StationsController(IUnitOfWork uow, IMapper imapper)
		{
			unitOfWork = uow;
			mapper = imapper;
		}

		// GET: api/Stations
		public IEnumerable<StationDto> GetStations()
        {
			return mapper.Map<List<Station>, List<StationDto>>(unitOfWork.StationRepository.GetAll().ToList());
        }

        // GET: api/Stations/5
        [ResponseType(typeof(Station))]
        public IHttpActionResult GetStation(int id)
        {
            Station station = unitOfWork.StationRepository.Get(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Stations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStation(int id, StationDto stationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stationDto.Id)
            {
                return BadRequest();
            }

			Station station = unitOfWork.StationRepository.Get(id);
			station.Name = stationDto.Name;
			station.Longitude = stationDto.Longitude;
			station.Latitude = stationDto.Latitude;
			
            try
            {
				unitOfWork.StationRepository.Update(station);
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(id))
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

        // POST: api/Stations
        [ResponseType(typeof(Station))]
        public IHttpActionResult PostStation(StationDto stationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			Station station = mapper.Map<StationDto, Station>(stationDto);
			unitOfWork.StationRepository.Add(station);
			unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = station.Id }, station);
        }

        // DELETE: api/Stations/5
        [ResponseType(typeof(Station))]
        public IHttpActionResult DeleteStation(int id)
        {
            Station station = unitOfWork.StationRepository.Get(id);
            if (station == null)
            {
                return NotFound();
            }

			unitOfWork.StationRepository.Remove(station);
			unitOfWork.Complete();

            return Ok(station);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationExists(int id)
        {
			return unitOfWork.StationRepository.Get(id) != null;
        }
    }
}