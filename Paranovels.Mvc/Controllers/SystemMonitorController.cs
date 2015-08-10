using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Thi.Core;

namespace Paranovels.Mvc.Controllers
{
    public class SystemMonitorController : Controller
    {
        // GET: SystemMonitor
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Email(string emailAddress = "fanslations+test@gmail.com")
        {
            var email = new MailMessage
            {
                Subject = "Testing message from System Monitor @ " + DateTime.Now,
                Body = "This is a test"
            };
            email.To.Add(emailAddress);

            var result = EmailService.Instance.CheckStatus();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}