using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebApp.Models.DomainModels;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.ModelRepositories
{
    public class TransportationLineRoutePointsRepository : Repository<TransportationLineRoutePoint, int>, ITransportationLineRoutePointsRepository
    {
		protected ApplicationDbContext ApplicationDbContext
		{
			get { return context as ApplicationDbContext; }
		}

		public override IEnumerable<TransportationLineRoutePoint> GetAll()
		{
			return ApplicationDbContext.TransportationLineRoutePoints
				.Include(x => x.Station)
				.Include(x => x.TransportationLine)
				.ToList();
		}

		public TransportationLineRoutePointsRepository(DbContext context) : base(context)
        {
        }
    }
}