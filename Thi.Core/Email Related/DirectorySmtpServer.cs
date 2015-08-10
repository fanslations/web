using System.Configuration;
using System.Net.Mail;

namespace Thi.Core
{
    public class DirectorySmtpServer : SmtpServer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectorySmtpServer"/> class.
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="successor">The successor.</param>
        public DirectorySmtpServer(string name, ISmtpServer successor = null)
        {
            Name = name;
            SmtpClient = new SmtpClient()
                             {
                                 DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                                 PickupDirectoryLocation = ConfigurationManager.AppSettings["PickupDirectory"],
                    
                             };
            Successor = successor;
        }

        #endregion
    }
}