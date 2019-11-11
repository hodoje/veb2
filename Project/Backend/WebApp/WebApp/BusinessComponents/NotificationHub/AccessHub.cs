using Microsoft.AspNet.SignalR;
using System;
using WebApp.Models;
using WebApp.Models.DomainModels;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using WebApp.Models.DomainModels.Dtos;

namespace WebApp.BusinessComponents.NotificationHubs
{
    [HubName("accessHub")]
    public class AccessHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<AccessHub>();

        private static object lockObject = new object();

        /// <summary>
        ///  Type parameters:
        //   Key:
        //     Unregistered User email.
        //
        //   Value:
        //     Context.Connection.Id
        /// </summary>
        private static Dictionary<string, string> UnregisteredUsers = new Dictionary<string, string>();

        /// <summary>
        /// Used to notify all connected admins that there is a new user for confirmation.
        /// </summary>
        /// <param name="user">User that just registered.</param>
        /// <returns><b>True</b> if user is successfully added to the collection for not registered users, otherwise <b>false</b>.</returns>
        public bool UserRegistered(ApplicationUserDto user)
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

                    hubContext.Clients.Group("Admins").userRegistered(userDto);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Registers user by removing him from collection which is for unregistered users and also it notifies all connected admins about the registration.
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
                    if (UnregisteredUsers.TryGetValue(userEmail, out string connectionId))
                    {
                        hubContext.Clients.Client(connectionId).confirmUser(userEmail);
                        UnregisteredUsers.Remove(userEmail);
                    }                    
                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

		/// <summary>
		/// Notifies the user waiting for registration and all the admins that the registration is declined.
		/// </summary>
		/// <param name="userEmail">Email of the user that confirmation is sent.</param>
		/// <returns><b>True</b> if declination is successfull, otherwise <b>false</b>.</returns>
		public bool DeclineRegistration(string userEmail)
        {
            try
            {
                lock (lockObject)
                {
                    hubContext.Clients.Group("Admins").declineUser(userEmail);
                    if (UnregisteredUsers.TryGetValue(userEmail, out string connectionId))
                    {
                        hubContext.Clients.Client(connectionId).declineUser(userEmail);
                        UnregisteredUsers.Remove(userEmail);
                    }
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
			// Only admins and users that haven't been registered will connect to this hub.
            if (Context.User.IsInRole("Admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");
            }
			else if (Context.User.IsInRole("AppUser"))
			{
				Groups.Add(Context.ConnectionId, "AppUser");
				if (!UnregisteredUsers.TryGetValue(Context.User.Identity.Name, out string connectionId))
				{
					UnregisteredUsers.Add(Context.User.Identity.Name, Context.ConnectionId);
				}
			}

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
			// We should not remove connections from Groups manually, framework deals with that
			//if (Context.User.IsInRole("Admin"))
			//{
			//	Groups.Remove(Context.ConnectionId, "Admins");
			//}
			if (UnregisteredUsers.TryGetValue(Context.User.Identity.Name, out string connectionId))
			{
				UnregisteredUsers.Remove(Context.User.Identity.Name);
			}

			return base.OnDisconnected(stopCalled);
        }
    }
}