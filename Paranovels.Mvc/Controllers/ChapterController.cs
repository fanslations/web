using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;

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

        public ActionResult NextChapter(NovelCriteria criteria, int? chapterID)
        {
            if (chapterID.HasValue)
            {
                var detail = Facade<NovelFacade>().GetNovel(criteria);
                var release = detail.Chapters.OrderBy(o => o.Volume).ThenBy(o=> o.Number).FirstOrDefault(w => w.ID > chapterID);
                if (release == null)
                    return View(detail);

                return RedirectPermanent(Url.Action("Detail", "Chapter", new { ID = release.ID, Seo = release.Title.ToSeo() }));
            }
            return null;
        }

        public ActionResult PreviousChapter(NovelCriteria criteria, int? chapterID)
        {
            if (chapterID.HasValue)
            {
                var detail = Facade<NovelFacade>().GetNovel(criteria);
                var release = detail.Chapters.OrderByDescending(o => o.Volume).ThenByDescending(o=>o.Number).FirstOrDefault(w => w.ID < chapterID);
                if (release == null)
                    return View(detail);

                return RedirectPermanent(Url.Action("Detail", "Chapter", new { ID = release.ID, Seo = release.Title.ToSeo() }));
            }
            return null;
        }

        public ActionResult Translating(ChapterCriteria criteria)
        {
            var detail = Facade<NovelFacade>().GetChapter(criteria);

            return View(detail);
        }

        public ActionResult Editing(ChapterCriteria criteria)
        {
            var detail = Facade<NovelFacade>().GetChapter(criteria);

            return View(detail);
        }

        public ActionResult Proofreading(ChapterCriteria criteria)
        {
            var detail = Facade<NovelFacade>().GetChapter(criteria);

            return View(detail);
        }


        public ActionResult Add(ChapterForm form)
        {
            return View(form);
        }

        public ActionResult Edit(ChapterCriteria criteria)
        {
            var model = Facade<NovelFacade>().GetChapter(criteria);
            return View(model);
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