using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;
using Thi.Web;

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
            var viewForm = new ViewForm { UserID = UserSession.UserID, SourceID = detail.ID, SourceTable = R.SourceTable.NOVEL };
            Facade<UserActionFacade>().Viewing(viewForm);

            return View(detail);
        }

        public ActionResult Add(NovelForm form)
        {
            return View(form);
        }

        [HttpPost]
        public JsonResult Form(NovelForm form, HttpPostedFileBase image, string imagePath)
        {
            if (image != null && image.ContentLength > 0)
            {
                imagePath = string.IsNullOrWhiteSpace(imagePath)
                    ? "/Fanslations/Novels/" + form.Title.ToSeo()
                    : imagePath;

                var driveService = GoogleDriveService.GetDriveService();
                var fileID = GoogleDriveService.uploadFile(driveService, image.InputStream, image.FileName, imagePath);
                form.ImageUrl = fileID;
            }
            return SaveChanges(form, Facade<NovelFacade>().AddNovel);
        }

        public ActionResult InlineEdit(InlineEditForm<NovelDetail> form)
        {
            form.Model = Facade<NovelFacade>().GetNovel(new NovelCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}