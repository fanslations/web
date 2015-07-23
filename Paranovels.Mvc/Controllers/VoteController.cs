using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class VoteController : SiteController
    {
        [HttpPost]
        public JsonResult Form(VoteForm form)
        {
            form.UserID = UserSession.UserID;
            var score = Facade<UserActionFacade>().Voting(form);
            return Json(score, JsonRequestBehavior.AllowGet);
        }
    }
}