using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents.NotificationHub
{
    [HubName("simulator")]
    public class SimulatorHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SimulatorHub>();

        private static List<VehicleModel> vehicles = new List<VehicleModel>();
        private static Timer timer = new Timer();

        private static bool eventSet = false;
        private static object lockObject = new object();

        static SimulatorHub()
        {
            timer.Interval = 10000;
            timer.Start();
        }

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

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            List<VehicleModel> currentVehicleState;
            foreach (var vehicle in vehicles)
            {
                vehicle.GetNextStop();
            }
            currentVehicleState = new List<VehicleModel>(vehicles);

            // TODO SEND TO ALL LISTNEERES
        }
    }
}