using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class RateController : SiteController
    {
        [HttpPost]
        public JsonResult Form(RateForm form)
        {
            form.UserID = UserSession.UserID;
            var score = Facade<UserActionFacade>().Rating(form);
            return Json(score, JsonRequestBehavior.AllowGet);
        }
    }
}