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
	[RoutePrefix("api/Schedules")]
	public class SchedulesController : ApiController
    {
		private IUnitOfWork unitOfWork;
		private IMapper mapper;

		public SchedulesController()
		{

		}

		public SchedulesController(IUnitOfWork uow, IMapper imapper)
		{
			unitOfWork = uow;
			mapper = imapper;
		}

		// GET: api/Schedules
		public IEnumerable<ScheduleDto> GetSchedules()
        {
			List<Schedule> schedules = unitOfWork.ScheduleRepository.GetAll().ToList();
			List<DayOfTheWeek> dayOfTheWeeks = unitOfWork.DayOfTheWeekRepository.GetAll().ToList();
			List<TransportationLine> transportationLines = unitOfWork.TransportationLineRepository.GetAllIncludeTransportationLineType().ToList();
			List<ScheduleDto> scheduleDtos = new List<ScheduleDto>();
			foreach (Schedule s in schedules)
			{
				TransportationLine transportationLine = transportationLines.FirstOrDefault(tl => tl.Id == s.TransportationLineId);
				s.TransportationLine = transportationLine;
				ScheduleDto scheduleDto = mapper.Map<Schedule, ScheduleDto>(s);
				scheduleDtos.Add(scheduleDto);
			}
			return scheduleDtos;
		}

        // GET: api/Schedules/5
        [ResponseType(typeof(Schedule))]
        public IHttpActionResult GetSchedule(int id)
        {
            Schedule schedule = unitOfWork.ScheduleRepository.Get(id);
            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        // PUT: api/Schedules/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSchedule(int id, ScheduleDto scheduleDto)
        {
			
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != scheduleDto.Id)
            {
                return BadRequest();
            }
			Schedule schedule = new Schedule();
			schedule.Id = scheduleDto.Id;
			schedule.Timetable = scheduleDto.Timetable;
			schedule.DayOfTheWeekId = unitOfWork.DayOfTheWeekRepository.Find(d => d.Name == scheduleDto.DayOfTheWeek).FirstOrDefault().Id;
			int lineNum = scheduleDto.LineNum;
			schedule.TransportationLineId = unitOfWork.TransportationLineRepository.Find(tl => tl.LineNum == lineNum).FirstOrDefault().Id;

            try
            {
				unitOfWork.ScheduleRepository.Update(schedule);
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(id))
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

        // POST: api/Schedules
        [ResponseType(typeof(Schedule))]
        public IHttpActionResult PostSchedule(Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.ScheduleRepository.Add(schedule);
			unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = schedule.Id }, schedule);
        }

        // DELETE: api/Schedules/5
        [ResponseType(typeof(Schedule))]
        public IHttpActionResult DeleteSchedule(int id)
        {
            Schedule schedule = unitOfWork.ScheduleRepository.Get(id);
            if (schedule == null)
            {
                return NotFound();
            }

			unitOfWork.ScheduleRepository.Remove(schedule);
			unitOfWork.Complete();

            return Ok(schedule);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ScheduleExists(int id)
        {
            return unitOfWork.ScheduleRepository.Get(id) != null;
        }
    }
}