//***
//  File name: ISmtpServer.cs
//  Purpose: This file contains method for smtp server.
//  Creation Date: 03/07/2012
//  Created by: Thi Nguyen
//  Modification Date:
//  Modified by:
//***

using System.Collections.Generic;
using System.Net.Mail;

namespace Thi.Core
{
    /// <summary>
    /// Interface of SmtpServer.
    /// </summary>
    public interface ISmtpServer
    {
        ISmtpServer Successor { get; set; }
        string Name { get; set; }
        #region Public Methods and Operators

        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="email">Email message</param>
        /// <returns>
        ///   <c>true</c> if success; <c>false</c> otherwise.
        /// </returns>
        bool Send(MailMessage email);

        IDictionary<string, string> CheckStatus();


        #endregion
    }
}