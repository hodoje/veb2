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
	public class PricelistRepository : Repository<Pricelist, int>, IPricelistRepository
	{
		protected ApplicationDbContext ApplicationDbContext
		{
			get { return context as ApplicationDbContext; }
		}

		public PricelistRepository(DbContext context) : base(context)
		{
		}

		public Pricelist GetActivePricelist()
		{
			return context.Set<Pricelist>().OrderByDescending(pl => pl.Id).FirstOrDefault();
		}
	}
}