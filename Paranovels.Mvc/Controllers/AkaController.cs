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
    public class AkaController : SiteController
    {
        [HttpPost]
        public JsonResult Form(AkaForm form)
        {
            return SaveChanges(form, Facade<AkaFacade>().AddAka);
        }

        public ActionResult InlineEdit(InlineEditForm<AkaDetail> form)
        {
            form.Model = Facade<AkaFacade>().Get(new TagCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}