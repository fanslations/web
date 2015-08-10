//***
//  File name: IEmailService.cs
//  Purpose: This file contains method for email service.
//  Creation Date: 03/07/2012
//  Created by: Thi Nguyen
//  Modification Date:
//  Modified by:
//***

using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;

namespace Thi.Core
{
    /// <summary>
    /// Interface of EmailService.
    /// </summary>
    public interface IEmailService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="email">Email message</param>
        /// <returns>
        ///   <c>true</c> if success; <c>false</c> otherwise.
        /// </returns>
        bool Send(MailMessage email);
        bool SendError(string error);
        IDictionary<string, string> CheckStatus();

        #endregion
    }
}