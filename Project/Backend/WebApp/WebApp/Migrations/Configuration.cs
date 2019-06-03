namespace WebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.DomainModels;
    using Models.DomainModels.Benefits;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Persistence.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApp.Persistence.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Controller"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Controller" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "AppUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppUser" };

                manager.Create(role);
            }

            PopulateUserTypesWithBenefits(context);

            PopulateUsers(context);

            PopulateAllTicketTables(context);


            context.SaveChanges();
        }

        private void PopulateUsers(WebApp.Persistence.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin@yahoo.com"))
            {
                UserType userType = context.UserTypes.FirstOrDefault(x => x.Name.Equals("Worker"));
                var user = new ApplicationUser() { Id = "admin", UserName = "admin@yahoo.com", Email = "admin@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Admin123!"), UserType = userType };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }
        }

        private void PopulateAllTicketTables(WebApp.Persistence.ApplicationDbContext context)
        {
            if (!context.Pricelists.Any(pl => pl.Id == 1))
            {
                Pricelist newPriceList = new Pricelist() { Id = 1, FromDate = new DateTime(2019, 1, 1), ToDate = new DateTime(2019, 12, 31, 23, 59, 59) };
                context.Pricelists.Add(newPriceList);
            }

            context.SaveChanges();

            Dictionary<string, int> ticketTypes = new Dictionary<string, int>(4)
            {
                {"Hourly",    100},
                {"Daily",      200},
                {"Monthly",   1500},
                {"Yearly",    5000}
            };

            Pricelist priceList = context.Pricelists.First();

            foreach (var ticketType in ticketTypes)
            {
                if (!context.TicketTypes.Any(x => x.Name.Equals(ticketType.Key)))
                {
                    TicketType newTicketType = new TicketType() { Name = ticketType.Key };
                    context.TicketTypes.Add(newTicketType);

                    context.TicketTypePricelists.Add(new TicketTypePricelist() { TicketType = newTicketType, Pricelist = priceList, BasePrice = ticketType.Value });
                }
            }
        }

        private void PopulateUserTypesWithBenefits(WebApp.Persistence.ApplicationDbContext context)
        {
            Dictionary<string, double> roleBenefits = new Dictionary<string, double>(3)
            {
                {"Student", 0.8 },
                {"Retiree", 0.85 },
                {"Worker", 1 }
            };

            foreach (var roleBenefit in roleBenefits)
            {
                string benefitName = $"{roleBenefit.Key}{typeof(TransportDiscountBenefit).ToString()}";

                if (!context.Benefits.Any(x => x.Name.Equals(benefitName)))
                { 
                    TransportDiscountBenefit benefit = new TransportDiscountBenefit() { Name = benefitName, CoefficientDiscount = roleBenefit.Value };
                    context.Benefits.Add(benefit);
                }

                context.SaveChanges();

                if (!context.UserTypes.Any(x => x.Name.Equals(roleBenefit.Key)))
                {
                    Benefit benefit = context.Benefits.First(x => x.Name.Equals(benefitName));

                    UserType userType = new UserType() { Name = roleBenefit.Key, Benefits = new List<Benefit>(1) { benefit } };
                    context.UserTypes.Add(userType);
                }
            }
        }
    }
}
