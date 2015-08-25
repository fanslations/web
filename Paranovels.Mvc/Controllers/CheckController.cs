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
    public class CheckController : SiteController
    {
        [HttpPost]
        public JsonResult Form(CheckForm form)
        {
            form.ByUserID = UserSession.UserID;
            var id = Facade<ConnectorFacade>().SaveCheck(form);
            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}