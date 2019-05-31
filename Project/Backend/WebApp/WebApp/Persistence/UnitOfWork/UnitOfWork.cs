using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Unity;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
      
        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        [Dependency]
        public IBenefitRepository BenefitRepository { get; private set; }
		[Dependency]
		public IUserTypeRepository UserTypeRepository { get; private set; }
		[Dependency]
		public IDayOfTheWeekRepository DayOfTheWeekRepository { get; private set; }
		[Dependency]
		public IPricelistRepository PricelistRepository { get; private set; }
		[Dependency]
		public IScheduleRepository ScheduleRepository { get; private set; }
		[Dependency]
		public IStationRepository StationRepository { get; private set; }
		[Dependency]
		public ITicketRepository TicketRepository { get; private set; }
		[Dependency]
		public ITicketTypePricelistRepository TicketTypePricelistRepository { get; private set; }
		[Dependency]
		public ITicketTypeRepository TicketTypeRepository { get; private set; }
		[Dependency]
		public ITransportationLineRepository TransportationLineRepository { get; private set; }

		public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}