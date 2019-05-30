using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Models;
using WebApp.Models.DomainModels;

namespace WebApp.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

		public virtual DbSet<Benefit> Benefits { get; set; }
		public virtual DbSet<DayOfTheWeek> DayOfTheWeeks { get; set; }
		public virtual DbSet<Pricelist> Pricelists { get; set; }
		public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
		public virtual DbSet<TicketTypePricelist> TicketTypePricelists { get; set; }
		public virtual DbSet<TicketType> TicketTypes { get; set; }
        public virtual DbSet<TransportationLine> TransportationLines { get; set; }
    }
}