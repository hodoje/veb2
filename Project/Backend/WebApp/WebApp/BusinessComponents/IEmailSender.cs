using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.BusinessComponents
{
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email with specified subject and body to destination.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="emailTo"></param>
        /// <returns><b>True</b> if e-mail is sent successfully, otherwise <b>false</b>.</returns>
        bool SendMail(string subject, string body, string emailTo);
    }
}