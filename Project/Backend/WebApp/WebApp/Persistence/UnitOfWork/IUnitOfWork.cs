using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBenefitRepository BenefitRepository { get; }
		IUserTypeRepository UserTypeRepository { get; }
		IDayOfTheWeekRepository DayOfTheWeekRepository { get; }
		IPricelistRepository PricelistRepository { get; }
		IScheduleRepository ScheduleRepository { get; }
        IStationRepository StationRepository { get; }
        ITicketRepository TicketRepository { get; }
		ITicketTypePricelistRepository TicketTypePricelistRepository { get; }
		ITicketTypeRepository TicketTypeRepository { get; }
        ITransportationLineRepository TransportationLineRepository { get; }
		ITransportationLineTypeRepository TransportationLineTypeRepository { get; }
		int Complete();
    }
}
