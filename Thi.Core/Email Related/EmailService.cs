//***
//  File name: EmailService.cs
//  Purpose: This file contains the implementation of IEmailService.
//  Creation Date: 03/07/2012
//  Created by: Thi Nguyen
//  Modification Date:
//  Modified by:
//***

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;

namespace Thi.Core
{
    /// <summary>
    /// The class implements the IEmailService.
    /// </summary>
    public class EmailService : IEmailService
    {
        #region Singleton
        private static IEmailService _instance;
        public static IEmailService Instance
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            get { return _instance ?? (_instance = new EmailService()); }
        }
        #endregion

        #region Constants and Fields

        private readonly string _mSystemName = ConfigurationManager.AppSettings["SystemEmailName"] ?? "Fanslations";
        private readonly string _mSystemEmail = ConfigurationManager.AppSettings["SystemEmailAddress"] ?? "donotreply@fanslations.com";
        private readonly string _mSystemEmailReply = ConfigurationManager.AppSettings["SystemEmailReplyAddress"] ?? "donotreply@fanslations.com";


        private readonly string _mSmtpServers = ConfigurationManager.AppSettings["SmtpServers"] ?? "gmail";

        private readonly ISmtpServer _mEmailServices = new DefaultSmtpServer("Default", null);

        #endregion

        #region Public Methods and Operators

        private EmailService()
        {
            _mEmailServices = SetSmtpServer(_mSmtpServers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="email">Email message</param>
        /// <returns>
        ///   <c>true</c> if success; <c>false</c> otherwise.
        /// </returns>
        public bool Send(MailMessage email)
        {
            // don't send to invalid email, just return false
            if (!email.To.Any())
                return false;

            if (email.From == null)
            {
                email.From = new MailAddress(_mSystemEmail, _mSystemName);
            }
            email.ReplyToList.Add(new MailAddress(_mSystemEmailReply, _mSystemName));
            email.IsBodyHtml = email.Body.Contains("</") || email.Body.Contains("/>");
            return this._mEmailServices.Send(email);
        }

        /// <summary>
        /// Sets the SMTP server.
        /// </summary>
        /// <param name="smtpServers">The SMTP servers.</param>
        /// <returns></returns>
        ISmtpServer SetSmtpServer(List<string> smtpServers)
        {
            if (smtpServers.Count == 0) return null;
            var smtpServer = smtpServers[0].Trim().ToLower();
            smtpServers.RemoveAt(0);
            switch (smtpServer)
            {
                case "gmail":
                    return new GmailSmtpServer(smtpServer, SetSmtpServer(smtpServers));
                case "local":
                    return new LocalSmtpServer(smtpServer, SetSmtpServer(smtpServers));
                case "directory":
                    return new DirectorySmtpServer(smtpServer, SetSmtpServer(smtpServers));
                case "default":
                    return new DefaultSmtpServer(smtpServer, SetSmtpServer(smtpServers));
                default:
                    return new DefaultSmtpServer("gmail");
            }
        }

        #endregion

        public IDictionary<string, string> CheckStatus()
        {
            var status = new Dictionary<string, string>();
            var emailServer = _mEmailServices;
            while (emailServer != null)
            {
                var result = emailServer.CheckStatus();
                status.Add(result.First().Key, result.First().Value);
                emailServer = emailServer.Successor;
            }
            return status;
        }

        public bool SendError(string errorMessage)
        {
            MailMessage email = new MailMessage
            {
                From = new MailAddress("donotreply@fanslations.com", "Fanslations.com"),
                Subject = "Error occured...",
                Priority = MailPriority.High
            };
            email.To.Add(ConfigurationManager.AppSettings["DevEmails"] ?? "thi517@gmail.com");
            email.Body = errorMessage;
            return Send(email);
        }
    }
}