using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Web;

namespace Paranovels.Mvc.Controllers
{
    public class SeriesController : SiteController
    {
        // GET: TranslationScene
        public ActionResult Index(SeriesCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            var pagedList = Facade<SearchFacade>().Search(searchModel);
            return View(pagedList);
        }

        public ActionResult Detail(SeriesCriteria criteria)
        {
            criteria.ByUserID = UserSession.UserID;
            var detail = Facade<SeriesFacade>().GetSeries(criteria);
            
            // log views
            Facade<UserActionFacade>().Viewing(UserSession.UserID, detail.SeriesID, R.SourceTable.SERIES);

            return View(detail);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Form(SeriesForm form, HttpPostedFileBase image, string imagePath)
        {
            var r = Request.QueryString;
            if (image != null && image.ContentLength > 0)
            {
                var driveService = GoogleDriveService.GetDriveService();
                var fileID = GoogleDriveService.uploadFile(driveService, image.InputStream, image.FileName, imagePath);
                form.ImageUrl = fileID;
            }
            return SaveChanges(form);
        }

        public ActionResult InlineEdit(InlineEditForm<SeriesDetail> form)
        {
            form.Model = Facade<SeriesFacade>().GetSeries(new SeriesCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }

        public JsonResult CheckFeedUpdate(int id = 5)
        {
            var results = Facade<FeedFacade>().CheckFeed(R.ConnectorType.SERIES_FEED, id);
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}