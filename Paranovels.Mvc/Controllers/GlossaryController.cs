using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class GlossaryController : SiteController
    {
        // GET: Glossary
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public JsonResult Form(GlossaryForm form)
        {

            return SaveChanges(form);
        }

        public ActionResult InlineEdit(InlineEditForm<GlossaryDetail> form)
        {
            form.Model = Facade<GlossaryFacade>().Get(new GlossaryCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}