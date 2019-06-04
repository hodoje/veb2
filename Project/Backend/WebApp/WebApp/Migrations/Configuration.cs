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
	using WebApp.Persistence;

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

			PopulateUserRoles(context);

			PopulateDaysOfTheWeek(context);

			PopulateTransporationTypes(context);

            PopulateStations(context);

			PopulateTransporationLines(context);

            PopulateSchedules(context);

			PopulateUserTypesWithBenefits(context);

			PopulateUsers(context);

			PopulateAllTicketTables(context);

			context.SaveChanges();
		}

        private void PopulateSchedules(ApplicationDbContext context)
        {
            bool sendTransaction = false;
            List<Schedule> schedules = new List<Schedule>(3)
                    {
                        new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Weekday"), Timetable = "7:00,7:15.8:00,8:30", TransportationLineId = context.TransportationLines.First().Id },
                        new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Saturday"), Timetable = "9:00,9:15.10:00,10:30", TransportationLineId = context.TransportationLines.First().Id },
                        new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Sunday"), Timetable = "11:00,11:15.12:00,12:30", TransportationLineId = context.TransportationLines.First().Id }
                    };

            foreach (Schedule schedule in schedules)
            {
                if (!context.Schedules.Any(x => x.DayOfTheWeek.Name.Equals(schedule.DayOfTheWeek.Name)))
                {
                    sendTransaction = true;
                    context.Schedules.Add(schedule);
                }
            }

            if (sendTransaction == true)
            {
                context.SaveChanges();
            }
        }

        private void PopulateTransporationLines(ApplicationDbContext context)
        {
            TransporationLineType tlType = context.TransporationLineTypes.First();
            Station station = context.Stations.First();

            if (!context.TransportationLines.Any(x => x.LineNum == 4))
            {
                TransportationLine transporationLine = new TransportationLine()
                {
                    LineNum = 4,
                    TransporationLineType = tlType,
                    Stations = new List<Station>(1) { station },
                };

                context.TransportationLines.Add(transporationLine);
            }

            context.SaveChanges();
        }

        private void PopulateTransporationTypes(ApplicationDbContext context)
		{
			List<string> transporationLineTypes = new List<string>(2) { "Town", "Suburban" };

			foreach (string type in transporationLineTypes)
			{
				if (!context.TransporationLineTypes.Any(x => x.Name.Equals(type)))
				{
					context.TransporationLineTypes.Add(new TransporationLineType() { Name = type });
				}
			}

			context.SaveChanges();
		}

		private void PopulateDaysOfTheWeek(ApplicationDbContext context)
		{
			List<string> daysOfTheWeek = new List<string>(3) { "Weekday", "Saturday", "Sunday" };

			foreach (string day in daysOfTheWeek)
			{
				if (!context.DayOfTheWeeks.Any(x => x.Name.Equals(day)))
				{
					context.DayOfTheWeeks.Add(new DayOfTheWeek() { Name = day });
				}
			}

			context.SaveChanges();
		}

		private void PopulateStations(ApplicationDbContext context)
		{
			if (!context.Stations.Any(x => x.Name.Equals("Station1")))
			{
				context.Stations.Add(new Station() { Name = "Station1", Longitude = 45.259302, Latitude = 19.832563 });
			}

			context.SaveChanges();
		}

		private void PopulateUserRoles(ApplicationDbContext context)
		{
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
		}

		private void PopulateUsers(ApplicationDbContext context)
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

        private void PopulateAllTicketTables(ApplicationDbContext context)
        {
            bool sendTransaction = false;
            if (!context.Pricelists.Any(pl => pl.Id == 1))
            {
                Pricelist newPriceList = new Pricelist() { Id = 1, FromDate = new DateTime(2019, 1, 1), ToDate = new DateTime(2019, 12, 31, 23, 59, 59) };
                context.Pricelists.Add(newPriceList);
                sendTransaction = true;
            }

            if (sendTransaction == true)
            {
                context.SaveChanges();
            }

            Dictionary<string, int> ticketTypes = new Dictionary<string, int>(4)
            {
                {"Hourly",     100},
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

        private void PopulateUserTypesWithBenefits(ApplicationDbContext context)
        {
            bool sendTransaction = false;
            Dictionary<string, double> roleBenefits = new Dictionary<string, double>(3)
            {
                {"Student", 0.8 },
                {"Retiree", 0.85 },
                {"Regular", 1 }
            };

            foreach (var roleBenefit in roleBenefits)
            {
                string benefitName = $"{roleBenefit.Key}{typeof(TransportDiscountBenefit).ToString()}";

                if (!context.Benefits.Any(x => x.Name.Equals(benefitName)))
                { 
                    TransportDiscountBenefit benefit = new TransportDiscountBenefit() { Name = benefitName, CoefficientDiscount = roleBenefit.Value };
                    context.Benefits.Add(benefit);

                    sendTransaction = true;
                }

                if (sendTransaction == true)
                {
                    context.SaveChanges();
                }

                sendTransaction = false;

                if (!context.UserTypes.Any(x => x.Name.Equals(roleBenefit.Key)))
                {
                    Benefit benefit = context.Benefits.First(x => x.Name.Equals(benefitName));

                    UserType userType = new UserType() { Name = roleBenefit.Key, Benefits = new List<Benefit>(1) { benefit } };
                    context.UserTypes.Add(userType);

                    sendTransaction = true;
                }

                
            }
        }
    }
}
