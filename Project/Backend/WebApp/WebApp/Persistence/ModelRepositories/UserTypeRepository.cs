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
	public class UserTypeRepository : Repository<UserType, int>, IUserTypeRepository
	{
		protected ApplicationDbContext ApplicationDbContext
		{
			get { return context as ApplicationDbContext; }
		}

        public override IEnumerable<UserType> GetAll()
        {
            return context.Set<UserType>().Include(x => x.Benefits)
                .ToList();
        }

        public UserTypeRepository(DbContext context) : base(context)
		{
		}
	}
}