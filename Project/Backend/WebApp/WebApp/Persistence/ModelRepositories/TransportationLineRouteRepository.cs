using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebApp.Models.DomainModels;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.ModelRepositories
{
    public class TransportationLineRouteRepository : Repository<TransportationLineRoute, int>, ITransportationLineRouteRepository
    {

		public override IEnumerable<TransportationLineRoute> GetAll()
		{
			return context.Set<TransportationLineRoute>()
				.Include(x => x.Station)
				.Include(x => x.TransporationLine)
				.ToList();
		}

		public TransportationLineRouteRepository(DbContext context) : base(context)
        {
        }
    }
}