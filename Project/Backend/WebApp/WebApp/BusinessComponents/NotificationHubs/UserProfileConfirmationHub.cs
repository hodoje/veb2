using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using WebApp.Models;
using WebApp.Models.Dtos;

namespace WebApp.BusinessComponents.NotificationHubs
{
    public class UserProfileConfirmationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<UserProfileConfirmationHub>();

        private static ReaderWriterLock readerWriterLock;
        private static List<string> notConfirmedUsers;

        private readonly int timeout = 5000;

        public UserProfileConfirmationHub()
        {
            readerWriterLock = new ReaderWriterLock();
            notConfirmedUsers = new List<string>();
        }


        /// <summary>
        /// Used to notify all connected admins that there is a new user for confirmation.
        /// </summary>
        /// <param name="userName">Username to confirm.</param>
        /// <returns><b>True</b> if user is successfully added to the collection for not registered users, otherwise <b>false</b>.</returns>
        public bool AddNewUser(ApplicationUser user)
        {
            try
            {
                readerWriterLock.AcquireWriterLock(timeout);

                notConfirmedUsers.Add(user.UserName);

                readerWriterLock.ReleaseWriterLock();

                ApplicationUserConfirmRegistrationDto userDto = user;

                hubContext.Clients.Group("Admin").newUser(userDto);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ConfirmRegistration(string userName)
        {
            bool userRegistered;
            try
            {
                readerWriterLock.AcquireWriterLock(timeout);

                userRegistered = notConfirmedUsers.Remove(userName);

                readerWriterLock.ReleaseWriterLock();



                return userRegistered;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}