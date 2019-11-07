using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.ModelRepositories
{
	public class ScheduleRepository : Repository<Schedule, int>, IScheduleRepository
	{
		protected ApplicationDbContext ApplicationDbContext
		{
			get { return context as ApplicationDbContext; }
		}

		public ScheduleRepository(DbContext context) : base(context)
		{
		}

		public IEnumerable<Schedule> GetAllIncludeDayOfTheWeek()
		{
			return ApplicationDbContext.Schedules.Include(s => s.DayOfTheWeek);
		}
	}
}