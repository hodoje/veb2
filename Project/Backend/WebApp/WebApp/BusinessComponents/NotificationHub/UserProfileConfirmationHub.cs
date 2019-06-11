using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebApp.Models;
using WebApp.Models.DomainModels;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApp.BusinessComponents.NotificationHubs
{
    [HubName("userConfirmation")]
    public class UserProfileConfirmationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<UserProfileConfirmationHub>();

        private static ReaderWriterLock readerWriterLock;

        private static List<ApplicationUserDto> notConfirmedUsers;

        private readonly int timeout = 5000;

        public UserProfileConfirmationHub()
        {
            readerWriterLock = new ReaderWriterLock();
            notConfirmedUsers = new List<ApplicationUserDto>();
        }

        /// <summary>
        /// Used to notify all connected admins that there is a new user for confirmation.
        /// </summary>
        /// <param name="user">User that just registered.</param>
        /// <returns><b>True</b> if user is successfully added to the collection for not registered users, otherwise <b>false</b>.</returns>
        public bool AddNewUser(ApplicationUser user)
        {
            try
            {
                readerWriterLock.AcquireWriterLock(timeout);

                ApplicationUserDto userDto = user;

                notConfirmedUsers.Add(userDto);
                
                hubContext.Clients.Group("Admin").newUser(userDto);

                readerWriterLock.ReleaseWriterLock();

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
            bool userRegistered;
            try
            {
                readerWriterLock.AcquireWriterLock(timeout);

                userRegistered = notConfirmedUsers.Remove(notConfirmedUsers.First(x => x.Email.Equals(userEmail)));

                hubContext.Clients.Group("Admins").userRegistered(userEmail);

                readerWriterLock.ReleaseWriterLock();

                return userRegistered;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public override Task OnConnected()
        {
            if (Context.User.IsInRole("Admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");

                readerWriterLock.AcquireReaderLock(timeout);

                Clients.Caller.usersToConfirm(notConfirmedUsers);

                readerWriterLock.ReleaseReaderLock();
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