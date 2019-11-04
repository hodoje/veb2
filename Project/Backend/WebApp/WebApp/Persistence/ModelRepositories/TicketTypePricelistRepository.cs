using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.ModelRepositories
{
	public class TicketTypePricelistRepository : Repository<TicketTypePricelist, int>, ITicketTypePricelistRepository
	{
		protected ApplicationDbContext ApplicationDbContext
		{
			get { return context as ApplicationDbContext; }
		}

		public TicketTypePricelistRepository(DbContext context) : base(context)
		{
		}

		public IEnumerable<TicketTypePricelist> GetAllIncludeTicketType()
		{
			return context.Set<TicketTypePricelist>().Include(ttpl => ttpl.TicketType);
		}

		public IEnumerable<TicketTypePricelist> FindIncludeTicketType(Expression<Func<TicketTypePricelist, bool>> predicate)
		{
			return context.Set<TicketTypePricelist>().Include(ttpl => ttpl.TicketType).Where(predicate);
		}
	}
}