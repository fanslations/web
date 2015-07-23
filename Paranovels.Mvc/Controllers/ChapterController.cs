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
            Facade<UserActionFacade>().Viewing(UserSession.UserID, detail.ChapterID, R.SourceTable.CHAPTER);

            return View(detail);
        }

        public ActionResult Translating(ChapterCriteria criteria)
        {
            var detail = Facade<NovelFacade>().GetChapter(criteria);

            return View(detail);
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