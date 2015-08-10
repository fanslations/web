using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.DataModels;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class FormController : SiteController
    {
        [HttpPost]
        public JsonResult Sticky(StickyForm form)
        {
            form.ByUserID = UserSession.UserID;
            var score = Facade<ConnectorFacade>().SaveSticky(form);
            return Json(score, JsonRequestBehavior.AllowGet);
        }
    }
}