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
    public class TransportationLineRepository : Repository<TransportationLine, int>, ITransportationLineRepository
    {
        protected ApplicationDbContext ApplicationDbContext
        {
            get { return context as ApplicationDbContext; }
        }

        public TransportationLineRepository(DbContext context) : base(context)
        {
        }

		public IEnumerable<TransportationLine> GetAllIncludeTransportationLineType()
		{
			return ApplicationDbContext.TransportationLines.Include(tl => tl.TransportationLineType);
		}

		public TransportationLine FindByIdIncludeRoutePoints(int id)
		{
			return ApplicationDbContext.TransportationLines.Include(tl => tl.TransportationLineRoutePoints).FirstOrDefault(tl => tl.Id == id);
		}

		public TransportationLine FindByLineNumberIncludeRoutePoints(int lineNum)
		{
			return ApplicationDbContext.TransportationLines.Include(tl => tl.TransportationLineRoutePoints).FirstOrDefault(tl => tl.LineNum == lineNum);
		}
	}
}