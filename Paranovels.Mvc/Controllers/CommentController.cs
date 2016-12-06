using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Mvc.Controllers
{
    public class CommentController : SiteController
    {
        // GET: Comment


        public ActionResult Index(CommentCriteria  criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            searchModel.PagedListConfig.PageSize = 9999;
            var pagedList = Facade<SearchFacade>().Search(searchModel);
            ViewBag.Criteria = criteria;
            return View(pagedList);
        }

        [HttpPost]
        public JsonResult Form(CommentForm form)
        {
            form.UserID = UserSession.UserID;
            return SaveChanges(form, Facade<CommentFacade>().AddComment);
        }

        public ActionResult InlineEdit(InlineEditForm<CommentDetail> form)
        {
            form.Model = Facade<CommentFacade>().Get(new CommentCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }

        [HttpPost]
        public JsonResult SpamReport(SpamReportForm form)
        {
            return SaveChanges(form, Facade<SpamReportFacade>().AddSpamReport);
        }
    }
}