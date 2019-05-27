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
        ILocationRepository LocationRepository { get; }
        IStationRepository StationRepository { get; }
        ITicketRepository TicketRepository { get; }
        ITransportationLineRepository TransportationLineRepository { get; }
        int Complete();
    }
}
