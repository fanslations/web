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
    public class ConnectorController : SiteController
    {
        [HttpPost]
        public JsonResult Form(ConnectorForm form)
        {
            form.ByUserID = UserSession.UserID;
            var score = Facade<ConnectorFacade>().AddConnector(form);
            return Json(score, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InlineEdit(InlineEditForm<ConnectorDetail> form)
        {
            form.Model = Facade<ConnectorFacade>().Get(new ConnectorCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}