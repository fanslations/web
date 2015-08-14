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
    public class ChapterController : SiteController
    {
        // GET: Chapter
        public ActionResult Index(ChapterCriteria criteria)
        {
            var detail = Facade<NovelFacade>().GetChapter(criteria);

            // log views
            var viewForm = new ViewForm { UserID = UserSession.UserID, SourceID = detail.ID, SourceTable = R.SourceTable.CHAPTER };
            Facade<UserActionFacade>().Viewing(viewForm);

            return View(detail);
        }

        public ActionResult Detail(ChapterCriteria criteria)
        {
            var detail = Facade<NovelFacade>().GetChapter(criteria);

            return View(detail);
        }

        public ActionResult Translating(ChapterCriteria criteria)
        {
            var detail = Facade<NovelFacade>().GetChapter(criteria);

            return View(detail);
        }

        public ActionResult Add(ChapterForm chapter)
        {
            return View(chapter);
        }

        [HttpPost]
        public JsonResult Form(ChapterForm chapter)
        {
            return SaveChanges(chapter);
        }

        public ActionResult InlineEdit(InlineEditForm<ChapterDetail> form)
        {
            form.Model = Facade<NovelFacade>().GetChapter(new ChapterCriteria {  ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}