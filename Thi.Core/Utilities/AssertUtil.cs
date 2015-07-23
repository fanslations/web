using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core.Utilities
{
    public class CommonUtils
    {
        public static void AssertTrue(bool trueValue, string errorMessage)
        {
            if (!trueValue)
                throw new ArgumentException(errorMessage);
        }

        public static string IssueTicket(object ticketData)
        {
            string ticket = string.Format("{0}|{1}", ticketData, DateTime.Now.Ticks);
            return EncryptionService.Instance.QuickEncrypt(ticket);
        }

        public static string IssueTicket(string url, object ticketData)
        {
            var ticketID = DateTime.Now.Ticks%100000000;
            string encryptedTicket = CommonUtils.IssueTicket(string.Format("{0}|{1}", ticketID, ticketData));
            return url.UrlAddQuery("TicketID", ticketID).UrlAddQuery("Ticket", encryptedTicket);
        }

        public static bool ValidateTicket(string encryptedTicket, object ticketID, int timeout = 5,
            bool throwInvalidException = false)
        {
            var ticket = ViewTicket(encryptedTicket);
            if (ticket == null) return false;
            var ticketParts = ticket.Split(new[] {'|'});
            var isIDMatched = ticketParts[0] == string.Format("{0}", ticketID);
            var isNotExpired = long.Parse(ticketParts[ticketParts.Length - 1]) >
                               DateTime.Now.AddSeconds(-1*timeout).Ticks;

            // build isValid
            var isValid = isIDMatched && isNotExpired;

            if (!isValid && throwInvalidException)
            {
                throw new ArgumentException("Ticket is invalid.");
            }
            return isValid;
        }

        public static string ViewTicket(string encryptedTicket)
        {
            try
            {
                return EncryptionService.Instance.QuickDecrypt(encryptedTicket);
                ;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}


