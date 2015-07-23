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
    public class UserListController : SiteController
    {
        public ActionResult Index(ListCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            var pagedList = Facade<ListFacade>().Search(searchModel);

            return View(pagedList);
        }

        public ActionResult Detail(ListCriteria criteria)
        {
            criteria.ByUserID = UserSession.UserID;
            var detail = Facade<ListFacade>().Get(criteria);

            return View(detail);
        }

        [HttpPost]
        public JsonResult Form(ListForm form)
        {
            form.UserID = UserSession.UserID;
            return SaveChanges(form);
        }

        //public ActionResult InlineEdit(InlineEditForm<ListDetail> form)
        //{
        //    form.Model = Facade<NovelFacade>().GetChapter(new ChapterCriteria { ID = form.ID });
        //    return View("_InlineEditPartial", form);
        //}
    }
}