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
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Text;
	using WebApp.Models;
    using WebApp.Models.Enumerations;
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

			try
			{
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
			catch(DbEntityValidationException e)
			{
				StringBuilder sb = new StringBuilder();

				foreach (var failure in e.EntityValidationErrors)
				{
					sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
					foreach (var error in failure.ValidationErrors)
					{
						sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
						sb.AppendLine();
					}
				}

				throw new DbEntityValidationException(
					"Entity Validation Failed - errors follow:\n" +
					sb.ToString(), e
				);
			}
		}

        private void PopulateSchedules(ApplicationDbContext context)
        {
            bool sendTransaction = false;
            List<Schedule> schedules = new List<Schedule>(6)
                    {
                        new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Weekday"), Timetable = "07:00,07:15.08:00,08:30", TransportationLineId = context.TransportationLines.First(tl => tl.LineNum == 4).Id },
                        new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Saturday"), Timetable = "09:00,09:15.10:00,10:30", TransportationLineId = context.TransportationLines.First(tl => tl.LineNum == 4).Id },
                        new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Sunday"), Timetable = "11:00,11:15.12:00,12:30", TransportationLineId = context.TransportationLines.First(tl => tl.LineNum == 4).Id },
						new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Weekday"), Timetable = "07:10,07:25.08:15,08:45", TransportationLineId = context.TransportationLines.First(tl => tl.LineNum == 70).Id },
						new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Saturday"), Timetable = "09:10,09:25.10:15,10:45", TransportationLineId = context.TransportationLines.First(tl => tl.LineNum == 70).Id },
						new Schedule() { DayOfTheWeek = context.DayOfTheWeeks.First(x => x.Name == "Sunday"), Timetable = "11:10,11:25.12:15,12:45", TransportationLineId = context.TransportationLines.First(tl => tl.LineNum == 70).Id }
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
			TransportationLineType tlType = context.TransporationLineTypes.First(tlt => tlt.Name == "Urban");
			TransportationLineType tlType2 = context.TransporationLineTypes.First(tlt => tlt.Name == "Suburban");
            Station station1 = context.Stations.First();
            Station station2 = context.Stations.FirstOrDefault(x => x.Name.Equals("Station2"));
            Station station3 = context.Stations.FirstOrDefault(x => x.Name.Equals("Station3"));

            TransportationLineRoute lineRoute = new TransportationLineRoute() { Station = station1, RoutePoint = 1 };
            TransportationLineRoute lineRoute2 = new TransportationLineRoute() { Station = station2, RoutePoint = 2 };

            if (!context.TransportationLines.Any(x => x.LineNum == 4))
            {
                TransportationLine transporationLine = new TransportationLine()
                {
                    LineNum = 4,
                    TransportationLineType = tlType,
                };

                context.TransportationLines.Add(transporationLine);

                context.SaveChanges();

                lineRoute.TransporationLine = transporationLine;
                lineRoute2.TransporationLine = transporationLine;
                context.TransportationLineRoutes.Add(lineRoute);
                context.TransportationLineRoutes.Add(lineRoute2);
            }


			if (!context.TransportationLines.Any(x => x.LineNum == 70))
			{
                lineRoute = new TransportationLineRoute() { Station = station1, RoutePoint = 1 };
                lineRoute2 = new TransportationLineRoute() { Station = station3, RoutePoint = 2 };

                TransportationLine transporationLine = new TransportationLine()
				{
					LineNum = 70,
					TransportationLineType = tlType2,
				};

				context.TransportationLines.Add(transporationLine);

                context.SaveChanges();

                lineRoute.TransporationLine = transporationLine;
                lineRoute2.TransporationLine = transporationLine;
                context.TransportationLineRoutes.Add(lineRoute);
                context.TransportationLineRoutes.Add(lineRoute2);
            }

			context.SaveChanges();
        }

        private void PopulateTransporationTypes(ApplicationDbContext context)
		{
			List<string> transporationLineTypes = new List<string>(2) { "Urban", "Suburban" };

			foreach (string type in transporationLineTypes)
			{
				if (!context.TransporationLineTypes.Any(x => x.Name.Equals(type)))
				{
					context.TransporationLineTypes.Add(new TransportationLineType() { Name = type });
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
				context.Stations.Add(new Station() { Name = "Station1", Latitude = 45.259302, Longitude = 19.832563 });
			}

            if (!context.Stations.Any(x => x.Name.Equals("Station2")))
            {
                context.Stations.Add(new Station() { Name = "Station2", Latitude = 45.252026, Longitude = 19.8368170 });
            }

            if (!context.Stations.Any(x => x.Name.Equals("Station3")))
            {
                context.Stations.Add(new Station() { Name = "Station3", Latitude = 45.24781110, Longitude = 19.8391915 });
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
                UserType userType = context.UserTypes.FirstOrDefault(x => x.Name.Equals("Regular"));
                var user = new ApplicationUser()
                {
                    Id = "admin",
                    UserName = "admin@yahoo.com",
                    Email = "admin@yahoo.com",
                    PasswordHash = ApplicationUser.HashPassword("Admin123!"),
                    UserType = userType,
                    Name = "joki",
                    LastName = "ziz",
                    Birthday = DateTime.Now,
                    Address = "zizova gajba",
                    RegistrationStatus = RegistrationStatus.Accepted
                };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }
        }

        private void PopulateAllTicketTables(ApplicationDbContext context)
        {
            bool sendTransaction = false;
            if (!context.Pricelists.Any(pl => pl.Id == 1))
            {
                Pricelist newPriceList = new Pricelist() { Id = 1, FromDate = new DateTime(2018, 1, 1) };
                context.Pricelists.Add(newPriceList);
                sendTransaction = true;
            }

            if (!context.Pricelists.Any(pl => pl.Id == 2))
            {
                Pricelist newPriceList = new Pricelist() { Id = 2, FromDate = new DateTime(2019, 1, 1) };
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

            int? lastPriceListId = context.Pricelists.Max(p => p.Id);
            Pricelist priceList = context.Pricelists.FirstOrDefault(p => p.Id == lastPriceListId);

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

                if (sendTransaction == true)
                {
                    context.SaveChanges();
                }
            }
        }
    }
}
