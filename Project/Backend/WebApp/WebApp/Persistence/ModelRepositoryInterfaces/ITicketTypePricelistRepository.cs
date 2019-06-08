using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.DomainModels;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.ModelRepositoryInterfaces
{
	public interface ITicketTypePricelistRepository : IRepository<TicketTypePricelist, int>
	{
		IEnumerable<TicketTypePricelist> GetAllIncludeTicketType();
		IEnumerable<TicketTypePricelist> FindIncludeTicketType(Expression<Func<TicketTypePricelist, bool>> predicate);
	}
}
