using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
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
        public IBenefitRepository BenefitRepository { get; set; }
		[Dependency]
		public IUserTypeRepository UserTypeRepository { get; set; }
		[Dependency]
		public IDayOfTheWeekRepository DayOfTheWeekRepository { get; set; }
		[Dependency]
		public IPricelistRepository PricelistRepository { get; set; }
		[Dependency]
		public IScheduleRepository ScheduleRepository { get; set; }
		[Dependency]
		public IStationRepository StationRepository { get; set; }
		[Dependency]
		public ITicketRepository TicketRepository { get; set; }
		[Dependency]
		public ITicketTypePricelistRepository TicketTypePricelistRepository { get; set; }
		[Dependency]
		public ITicketTypeRepository TicketTypeRepository { get; set; }
		[Dependency]
		public ITransportationLineRepository TransportationLineRepository { get; set; }
		[Dependency]
		public ITransportationLineTypeRepository TransportationLineTypeRepository { get; set; }
        [Dependency]
        public ITransportationLineRoutePointsRepository TransportationLineRoutePointsRepository { get; set; }
		[Dependency]
		public ITransactionRepository TransactionRepository { get; set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<IdentityRole> GetAllRolesIncludeUsers()
        {
            return (_context as ApplicationDbContext).Roles.Include(role => role.Users).ToList();
        }
    }
}