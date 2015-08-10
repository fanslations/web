//***
//  File name: GmailSmtpServer.cs
//  Purpose: This file contains the implementation of GmailSmtpServer.
//  Creation Date: 03/07/2012
//  Created by: Thi Nguyen
//  Modification Date:
//  Modified by:
//***

using System.Configuration;
using System.Net.Mail;

namespace Thi.Core
{
    /// <summary>
    /// Local smtp server.
    /// </summary>
    public class LocalSmtpServer : SmtpServer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalSmtpServer"/> class.
        /// </summary>
        /// <param name="successor">The successor.</param>
        /// <param name="name"> </param>
        public LocalSmtpServer(string name, ISmtpServer successor = null)
        {
            Name = name;
            SmtpClient = new SmtpClient
                             {
                                 Host = ConfigurationManager.AppSettings["SmtpServerLocal"],
                                 Port = 25,
                                 EnableSsl = false,
                                 DeliveryMethod = SmtpDeliveryMethod.Network
                             };
            Successor = successor;
        }

        #endregion


    }
}