using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.ModelRepositoryInterfaces
{
    public interface ITransportationLineRouteRepository : IRepository<TransportationLineRoute, int>
    {
    }
}