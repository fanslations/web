//***
//  File name: GmailSmtpServer.cs
//  Purpose: This file contains the implementation of GmailSmtpServer.
//  Creation Date: 03/07/2012
//  Created by: Thi Nguyen
//  Modification Date:
//  Modified by:
//***

using System.Net;
using System.Net.Mail;

namespace Thi.Core
{
    /// <summary>
    /// Gmail smtp server.
    /// </summary>
    public class GmailSmtpServer : SmtpServer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GmailSmtpServer"/> class.
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="successor">The successor.</param>
        public GmailSmtpServer(string name, ISmtpServer successor = null)
        {
            Name = name;
            SmtpClient = new SmtpClient("smtp.gmail.com");
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.EnableSsl = true;
            SmtpClient.Credentials = new NetworkCredential("thi.email.service@gmail.com", "Thi@Email@Service");
            SmtpClient.Port = 587;
            Successor = successor;
        }

        #endregion
    }
}