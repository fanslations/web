using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Web;

namespace Paranovels.Mvc.Controllers
{
    public class TranslatingController : SiteController
    {
        public ActionResult Index()
        {
           
            return View();
        }

        public JsonResult BingTranslator(TranslatorCriteria criteria)
        {
            var translated = Facade<TranslatorFacade>().Translate(criteria);
            return Json(translated, JsonRequestBehavior.AllowGet);
        }
	}
}