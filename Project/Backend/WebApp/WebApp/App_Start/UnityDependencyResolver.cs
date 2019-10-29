using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using WebApp.App_Start.MappingProfiles;
using WebApp.BusinessComponents;
using WebApp.BusinessComponents.NotificationHubs;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.ModelRepositories;
using WebApp.Persistence.ModelRepositoryInterfaces;
using WebApp.Persistence.UnitOfWork;
using WebApp.Providers;

namespace WebApp.App_Start
{
    public class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void RegisterTypes()
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
           
            // PerResolveLifetimeManager will create a new context each time a context is requested
            // BUT if there is a hierarchy of objects (child uses the same context as the parent etc.)
            // all of them will get the same context
            container.RegisterType<DbContext, ApplicationDbContext>(new PerResolveLifetimeManager());
            container.RegisterType<IBenefitRepository, BenefitRepository>();
			container.RegisterType<IUserTypeRepository, UserTypeRepository>();
			container.RegisterType<IDayOfTheWeekRepository, DayOfTheWeekRepository>();
			container.RegisterType<IPricelistRepository, PricelistRepository>();
			container.RegisterType<IScheduleRepository, ScheduleRepository>();
			container.RegisterType<IStationRepository, StationRepository>();
			container.RegisterType<ITicketRepository, TicketRepository>();
			container.RegisterType<ITicketTypePricelistRepository, TicketTypePricelistRepository>();
			container.RegisterType<ITicketTypeRepository, TicketTypeRepository>();
			container.RegisterType<ITransportationLineRepository, TransportationLineRepository>();
			container.RegisterType<ITransportationLineTypeRepository, TransportationLineTypeRepository>();
			container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<ITicketBusinessComponent, TicketBusinessComponent>();
            container.RegisterType<IEmailSender, SMTPClient>();
            container.RegisterType<ITransportationLineRouteRepository, TransportationLineRouteRepository>();
            container.RegisterType<ITransportationLineComponent, TransportationLineComponent>();
			container.RegisterType<IPricelistComponent, PricelistComponent>();
            container.RegisterType<UserProfileConfirmationHub>();
            // This allows usage of UnitOfWork in AccountController
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<ISecureDataFormat<AuthenticationTicket>, CustomJwtFormat>(new InjectionConstructor("http://localhost:52296"));
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new InjectionConstructor(typeof(ApplicationDbContext)));

			MapperConfiguration config = new MapperConfiguration(c =>
			{
				c.AddProfile<DayOfTheWeekMappingProfile>();
				c.AddProfile<TransportationLineTypeMappingProfile>();
				c.AddProfile<TransportationLineMappingProfile>();
				c.AddProfile<ScheduleMappingProfile>();
                c.AddProfile<UserTypeMappingProfile>();
				c.AddProfile<PricelistMappingProfile>();
                c.AddProfile<ApplicationUserMappingProfile>();
				c.AddProfile<StationMappingProfile>();
			});

			container.RegisterType<IMapper, Mapper>(new InjectionConstructor(config));

		}

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            container.Dispose();
        }
    }
}