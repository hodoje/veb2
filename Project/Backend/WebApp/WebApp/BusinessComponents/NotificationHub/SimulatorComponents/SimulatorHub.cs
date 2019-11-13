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
		private static IUnitOfWork unitOfWork;
		private static ITransportationLineComponent transportationLineComponent;
		private static Dictionary<string, string> onlineListeners = new Dictionary<string, string>();

		static SimulatorHub()
        {
			unitOfWork = (IUnitOfWork)GlobalHost.DependencyResolver.GetService(typeof(IUnitOfWork));
			transportationLineComponent = (ITransportationLineComponent)GlobalHost.DependencyResolver.GetService(typeof(ITransportationLineComponent));
		}

		public void StartSimulation()
		{
			timer.Interval = 1000;
			timer.Start();
			timer.Elapsed += SendSimulation;
		}

		private void SendSimulation(object sender, ElapsedEventArgs e)
		{
			// Don't waste resources if there are no listeners
			//if(onlineListeners.Count > 0)
			//{
				UpdateVehiclePositions();
			//}
		}

		public void UpdateVehiclePositions()
		{
			List<VehicleModel> currentVehicleState = new List<VehicleModel>();
			List<VehicleModel> vehicles = CreateNotificationModel();

			Clients.Group("Listeners").vehiclesChangedPositions(vehicles);
		}

		private List<VehicleModel> CreateNotificationModel()
		{
			Random random = new Random();
			List<int> existingLines = unitOfWork.TransportationLineRepository.GetAll().Select(x => x.LineNum).ToList();
			List<VehicleModel> vehicles = new List<VehicleModel>(existingLines.Count);

			foreach (var line in existingLines)
			{
				TransportationLinePlanDto lineDto = transportationLineComponent.GetTransportationLinePlan(unitOfWork, line);

				if(lineDto.RoutePoints.Count > 0)
				{
					Station currStation = lineDto.RoutePoints[random.Next(0, lineDto.RoutePoints.Count)].Station;
					vehicles.Add(new VehicleModel(line, new CurrentPosition() { Latitude = currStation.Latitude, Longitude = currStation.Longitude }));
				}
			}

			return vehicles;
		}
		public override Task OnConnected()
		{
			Groups.Add(Context.ConnectionId, "Listeners");
			onlineListeners.Add(Context.User.Identity.Name, Context.ConnectionId);

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			Groups.Remove(Context.ConnectionId, "Listeners");
			onlineListeners.Remove(Context.User.Identity.Name);

			return base.OnDisconnected(stopCalled);
		}
	}
}