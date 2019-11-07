using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Linq;
using WebApp.Models.DomainModels;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.ModelRepositories
{
	public class PricelistRepository : Repository<Pricelist, int>, IPricelistRepository
	{
		protected ApplicationDbContext ApplicationDbContext
		{
			get { return context as ApplicationDbContext; }
		}

		public PricelistRepository(DbContext context) : base(context)
		{
		}

		public override IEnumerable<Pricelist> GetAll()
		{
			return ApplicationDbContext.Pricelists.Include(x => x.TicketTypePricelists).ToList();
		}

		public Pricelist GetActivePricelist()
		{
			return ApplicationDbContext.Pricelists.OrderByDescending(pl => pl.Id).FirstOrDefault();
		}
	}
}