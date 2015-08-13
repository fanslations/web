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
            return View(pagedList);
        }

        public ActionResult Detail(GroupCriteria criteria)
        {
            var detail = Facade<GroupFacade>().GetGroup(criteria);

            // log views
            var viewForm = new ViewForm { UserID = UserSession.UserID, SourceID = detail.GroupID, SourceTable = R.SourceTable.GROUP };
            Facade<UserActionFacade>().Viewing(viewForm);

            return View(detail);
        }

        public JsonResult CheckFeedUpdate(int id = 5)
        {
            var results = Facade<FeedFacade>().CheckFeed(R.ConnectorType.GROUP_FEED, id);
            return Json(results, JsonRequestBehavior.AllowGet);
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
            return SaveChanges(form);
        }

        public ActionResult InlineEdit(InlineEditForm<GroupDetail> form)
        {
            form.Model = Facade<GroupFacade>().GetGroup(new GroupCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}