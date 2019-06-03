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
	public class TicketTypeRepository : Repository<TicketType, int>, ITicketTypeRepository
	{
		protected ApplicationDbContext ApplicationDbContext
		{
			get { return context as ApplicationDbContext; }
		}

        public override TicketType Get(int id)
        {
            return context.Set<TicketType>().Include(tt => tt.TicketTypePricelists).FirstOrDefault(t => t.Id == id);
        }

        public TicketTypeRepository(DbContext context) : base(context)
		{
		}
	}
}