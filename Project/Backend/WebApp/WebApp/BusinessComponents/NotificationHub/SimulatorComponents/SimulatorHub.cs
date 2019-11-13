using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using WebApp.BusinessComponents.NotificationHub.SimulatorComponents;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents.NotificationHub.SimulatorComponents
{
    [HubName("simulatorHub")]
    public class SimulatorHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SimulatorHub>();
        private static Timer timer = new Timer();
		private static ITransportationLineComponent transportationLineComponent;
		private static HashSet<string> onlineListeners = new HashSet<string>();

		static SimulatorHub()
        {
			transportationLineComponent = (ITransportationLineComponent)GlobalHost.DependencyResolver.GetService(typeof(ITransportationLineComponent));
			StartSimulation();
		}

		public static void StartSimulation()
		{
			if (!timer.Enabled)
			{
				timer.Interval = 1000;
				timer.Start();
				timer.Elapsed += SendSimulation;
			}
		}

		private static void SendSimulation(object sender, ElapsedEventArgs e)
		{
			// Don't waste resources if there are no listeners
			if (onlineListeners.Count > 0)
			{
				UpdateVehiclePositions();
			}
		}

		public static void UpdateVehiclePositions()
		{
			List<VehicleModel> currentVehicleState = new List<VehicleModel>();
			List<VehicleModel> vehicles = CreateNotificationModel();

			hubContext.Clients.Group("Listeners").vehiclesChangedPositions(vehicles);
		}

		private static List<VehicleModel> CreateNotificationModel()
		{
			Random random = new Random();
			using(UnitOfWork uow = (UnitOfWork)GlobalHost.DependencyResolver.GetService(typeof(IUnitOfWork)))
			{
				List<int> existingLines = uow.TransportationLineRepository.GetAll().Select(x => x.LineNum).ToList();
				List<VehicleModel> vehicles = new List<VehicleModel>(existingLines.Count);

				foreach (var line in existingLines)
				{
					TransportationLinePlanDto lineDto = transportationLineComponent.GetTransportationLinePlan(uow, line);

					if (lineDto.RoutePoints.Count > 0)
					{
						Station currStation = lineDto.RoutePoints[random.Next(0, lineDto.RoutePoints.Count)].Station;
						vehicles.Add(new VehicleModel(line, new CurrentPosition() { Latitude = currStation.Latitude, Longitude = currStation.Longitude }));
					}
				}

				return vehicles;
			}
		}
		public override Task OnConnected()
		{
			Groups.Add(Context.ConnectionId, "Listeners");
			onlineListeners.Add(Context.ConnectionId);

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			Groups.Remove(Context.ConnectionId, "Listeners");
			onlineListeners.Remove(Context.ConnectionId);

			return base.OnDisconnected(stopCalled);
		}
	}
}