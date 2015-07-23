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
            var score = Facade<ConnectorFacade>().AddConnector(form);
            return Json(score, JsonRequestBehavior.AllowGet);
        }
    }
}