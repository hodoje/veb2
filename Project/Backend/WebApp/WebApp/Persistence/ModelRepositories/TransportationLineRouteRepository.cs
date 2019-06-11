using System.Data.Entity;
using WebApp.Models.DomainModels;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.ModelRepositories
{
    public class TransportationLineRouteRepository : Repository<TransportationLineRoute, int>, ITransportationLineRouteRepository
    {
        public TransportationLineRouteRepository(DbContext context) : base(context)
        {
        }
    }
}