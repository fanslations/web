using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;
using Thi.Web;

namespace Paranovels.Mvc.Controllers
{
    public class GroupController : SiteController
    {
        // GET: Group
        public ActionResult Index(GroupCriteria criteria)
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
                    Title = s.Name,
                    Url = Url.Action("Detail", "Group", new {ID = s.ID, Seo = s.Name.ToSeo()}),
                });
                return FeedGenerator(feedItems, criteria.Alt);
            }

            return View(pagedList);
        }

        public ActionResult Detail(GroupCriteria criteria)
        {
            criteria.ByUserID = UserSession.UserID;
            var detail = Facade<GroupFacade>().GetGroup(criteria);

            // log views
            var viewForm = new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.GROUP };
            Facade<UserActionFacade>().Viewing(viewForm);

            return View(detail);
        }

        public ActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public JsonResult Form(GroupForm form, HttpPostedFileBase image, string imagePath)
        {
            if (image != null && image.ContentLength > 0)
            {
                var driveService = GoogleDriveService.GetDriveService();
                var fileID = GoogleDriveService.uploadFile(driveService, image.InputStream, image.FileName, imagePath);
                form.ImageUrl = fileID;
            }
            return SaveChanges(form, Facade<GroupFacade>().AddGroup);
        }

        public ActionResult InlineEdit(InlineEditForm<GroupDetail> form)
        {
            form.Model = Facade<GroupFacade>().GetGroup(new GroupCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }

        public JsonResult CheckFeedUpdate(int id)
        {
            var results = Facade<FeedFacade>().CheckFeed(R.ConnectorType.GROUP_FEED, id);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckFeed(string feedUrl)
        {
            var releases = Facade<FeedFacade>().GetNewReleases(feedUrl);
            return Json(releases, JsonRequestBehavior.AllowGet);
        }
    }
}