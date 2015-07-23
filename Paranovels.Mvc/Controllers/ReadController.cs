using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class ReadController : SiteController
    {
        [HttpPost]
        public JsonResult Form(ReadForm form)
        {
            form.UserID = UserSession.UserID;
            var read = Facade<UserActionFacade>().Reading(form);
            return Json(read, JsonRequestBehavior.AllowGet);
        }
    }
}