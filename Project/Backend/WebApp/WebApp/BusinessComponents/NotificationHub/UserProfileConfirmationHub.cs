using Microsoft.AspNet.SignalR;
using System;
using WebApp.Models;
using WebApp.Models.DomainModels;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApp.BusinessComponents.NotificationHubs
{
    [HubName("userConfirmation")]
    public class UserProfileConfirmationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<UserProfileConfirmationHub>();

        private static object lockObject = new object();


        /// <summary>
        /// Used to notify all connected admins that there is a new user for confirmation.
        /// </summary>
        /// <param name="user">User that just registered.</param>
        /// <returns><b>True</b> if user is successfully added to the collection for not registered users, otherwise <b>false</b>.</returns>
        public bool AddNewUser(ApplicationUser user)
        {
            try
            {
                lock (lockObject)
                {
                    ApplicationUserDto userDto = user;
                    var jsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    var interimObject = JsonConvert.DeserializeObject<ApplicationUserDto>(JsonConvert.SerializeObject(userDto));
                    var myJsonOutput = JsonConvert.SerializeObject(interimObject, jsonSerializerSettings);

                    hubContext.Clients.Group("Admins").newUser(userDto);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Registeres user by removing him from collection which is for unregistered users and also it notifies all connected admins about the registration.
        /// </summary>
        /// <param name="userEmail">Email of the user that confirmation is sent.</param>
        /// <returns><b>True</b> if confirmation is successfull, otherwise <b>false</b>.</returns>
        public bool ConfirmRegistration(string userEmail)
        {
            try
            {
                lock (lockObject)
                {
                    hubContext.Clients.Group("Admins").confirmUser(userEmail);
                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool DeclineRegistration(string userEmail)
        {
            try
            {
                lock (lockObject)
                {
                    hubContext.Clients.Group("Admins").declineUser(userEmail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool BanUser(string userEmail)
        {
            try
            {
                lock (lockObject)
                {
                    hubContext.Clients.Group("Admins").banUser(userEmail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UnbanUser(string userEmail)
        {
            try
            {
                lock (lockObject)
                {
                    hubContext.Clients.Group("Admins").unbanUser(userEmail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override Task OnConnected()
        {
            if (Context.User.IsInRole("Admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.IsInRole("Admin"))
            {
                Groups.Remove(Context.ConnectionId, "Admins");
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}