using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;
using Thi.Web;

namespace Paranovels.Mvc.Controllers
{
    public class SeriesController : SiteController
    {
        public ActionResult Index(SeriesCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            var pagedList = Facade<SearchFacade>().Search(searchModel);

            // alternative version
            if (!string.IsNullOrWhiteSpace(criteria.Alt))
            {
                var feedItems = pagedList.Data.Select(s => new FeedGrid
                {
                    InsertedDate = s.InsertedDate,
                    UpdatedDate = s.UpdatedDate,
                    Title = s.Title,
                    Url = Url.Action("Detail", "Series", new { ID = s.ID, Seo = s.Title.ToSeo() }),
                    Content = s.Synopsis
                });
                return FeedGenerator(feedItems, criteria.Alt);
            }

            return View(pagedList);
        }

        public ActionResult Detail(SeriesCriteria criteria)
        {
            criteria.ByUserID = UserSession.UserID;
            var detail = Facade<SeriesFacade>().GetSeries(criteria);
            
            // log views
            var viewForm = new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.SERIES };
            Facade<UserActionFacade>().Viewing(viewForm);

            return View(detail);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Form(SeriesForm form, HttpPostedFileBase image, string imagePath)
        {
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

        public JsonResult CheckFeed(string feedUrl)
        {
            var releases = Facade<FeedFacade>().GetNewReleases(feedUrl);
            return Json(releases, JsonRequestBehavior.AllowGet);
        }
    }
}