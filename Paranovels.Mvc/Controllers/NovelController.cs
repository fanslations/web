using System.Globalization;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Mvc.Controllers
{
    public class NovelController : SiteController
    {
        public ActionResult Index(NovelCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            var pagedList = Facade<SearchFacade>().Search(searchModel);
            return View(pagedList);
        }
        public ActionResult Detail(NovelCriteria criteria)
        {
            var detail = Facade<NovelFacade>().GetNovel(criteria);

            // log views
            Facade<UserActionFacade>().Viewing(UserSession.UserID, detail.NovelID, R.SourceTable.NOVEL);

            return View(detail);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Form(NovelForm novel)
        {
            return SaveChanges(novel);
        }

        public ActionResult InlineEdit(InlineEditForm<NovelDetail> form)
        {
            form.Model = Facade<NovelFacade>().GetNovel(new NovelCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}