using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class ContentController : SiteController
    {
        // GET: Content
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Form(ContentForm chapter)
        {
            return SaveChanges(chapter);
        }

        public ActionResult InlineEdit(InlineEditForm<ChapterDetail> form)
        {
            form.Model = Facade<NovelFacade>().GetChapter(new ChapterCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}