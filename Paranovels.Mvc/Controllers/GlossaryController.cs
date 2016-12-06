using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class GlossaryController : SiteController
    {
        public ActionResult Index(GlossaryCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            if (criteria.IDToInt > 0)
            {
                criteria.RawLanguages = new[] {criteria.IDToInt};
                criteria.ID = "";
            }
            var pagedList = Facade<GlossaryFacade>().Search(searchModel);
            return View(pagedList);
        }

        public ActionResult Detail(GlossaryCriteria criteria)
        {
            criteria.ByUserID = UserSession.UserID;
            var detail = Facade<GlossaryFacade>().Get(criteria);

            // log views
            var viewForm = new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.GLOSSARY };
            Facade<UserActionFacade>().Viewing(viewForm);

            return View(detail);
        }

        public ActionResult Add(GlossaryForm form)
        {
            return View(form);
        }

        [HttpPost]
        public JsonResult Form(GlossaryForm form)
        {
            return SaveChanges(form, Facade<GlossaryFacade>().AddGlossary);
        }

        public ActionResult InlineEdit(InlineEditForm<GlossaryDetail> form)
        {
            form.Model = Facade<GlossaryFacade>().Get(new GlossaryCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}