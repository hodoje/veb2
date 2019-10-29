using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents.NotificationHub
{
    [HubName("simulator")]
    public class SimulatorHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SimulatorHub>();

        private static Timer timer = new Timer();

        private static bool eventSet = false;
        private static object lockObject = new object();

		private static IUnitOfWork unitOfWork;
		private static ITransportationLineComponent transporationLineComponent;

		static SimulatorHub()
        {
            timer.Interval = 3000;
            timer.Start();

			unitOfWork = (IUnitOfWork)GlobalHost.DependencyResolver.GetService(typeof(IUnitOfWork));
			transporationLineComponent = (ITransportationLineComponent)GlobalHost.DependencyResolver.GetService(typeof(ITransportationLineComponent));
		}

		//public SimulatorHub(IUnitOfWork unitOfWork, ITransporationLineComponent transporationLineComponent)
		//{
		//	this.unitOfWork = unitOfWork;
		//	this.transporationLineComponent = transporationLineComponent;
		//}

		public void CreateEvent()
		{
			lock (lockObject)
			{
				if (!eventSet)
				{
					timer.Elapsed += OnTimedEvent;
					eventSet = true;
				}
			}
		}

		public override Task OnConnected()
		{
			Groups.Add(Context.ConnectionId, "Listeners");

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			Groups.Remove(Context.ConnectionId, "Listeners");

			return base.OnDisconnected(stopCalled);
		}

		public void SendMessage()
		{
			Clients.Group("Listeners").vehicleChangePosition();
		}

		public void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			List<VehicleModel> currentVehicleState = new List<VehicleModel>();

			List<VehicleModel> vehicles = CreateNotificationModel();

			Clients.Group("Listeners").vehicleChangePosition(vehicles);
		}

		private List<VehicleModel> CreateNotificationModel()
		{
			Random random = new Random();
			List<int> existingLines = unitOfWork.TransportationLineRepository.GetAll().Select(x => x.LineNum).ToList();
			List<VehicleModel> vehicles = new List<VehicleModel>(existingLines.Count);

			foreach (var line in existingLines)
			{
				TransportationLinePlanDto lineDto = transporationLineComponent.GetTransporationLinePlan(unitOfWork, line);

				Station currStation = lineDto.Routes[random.Next(0, lineDto.Routes.Count)].Station;
				vehicles.Add(new VehicleModel(line, new CurrentPosition() { Latitude = currStation.Latitude, Longitude = currStation.Longitude }));
			}

			return vehicles;
		}
	}
}