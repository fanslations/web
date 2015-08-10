//***
//  File name: SmtpServer.cs
//  Purpose: This file contains the implementation of ISmtpServer.
//  Creation Date: 03/07/2012
//  Created by: Thi Nguyen
//  Modification Date:
//  Modified by:
//***

using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Thi.Core
{
    /// <summary>
    /// Abstract Smtp Server.
    /// </summary>
    public abstract class SmtpServer : ISmtpServer
    {
        #region Properties

        protected SmtpClient SmtpClient { get; set; }
        public ISmtpServer Successor { get; set; }
        public string Name { get; set; }
        private Object lockThis = new Object();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="email">Email message</param>
        /// <returns>
        ///   <c>true</c> if success; <c>false</c> otherwise.
        /// </returns>
        public bool Send(MailMessage email)
        {
            lock (lockThis)
            {
                try
                {
                    SmtpClient.Send(email);
                    return true;
                }
                catch (Exception ex)
                {
                    ex.Data.Clear(); // suppress exception
                    if (Successor == null) return false;
                    return Successor.Send(email);
                }
            }
        }

        public IDictionary<string, string> CheckStatus()
        {
            var status = "";
            var subject = string.Format("Check status @ {0}", DateTime.Now);
            try
            {
                SmtpClient.Send(new MailMessage("thi.email.service+checkstatus@gmail.com", "thi.email.service@gmail.com", subject, subject));
                status = "OK";
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return new Dictionary<string, string>
                       {
                           {Name, status}
                       };
        }

        #endregion
    }
}