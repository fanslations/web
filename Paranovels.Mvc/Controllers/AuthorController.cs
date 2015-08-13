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
    public class AuthorController : SiteController
    {
        // GET: Group
        public ActionResult Index(AuthorCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            var pagedList = Facade<AuthorFacade>().Search(searchModel);
            return View(pagedList);
        }

        public ActionResult Detail(AuthorCriteria criteria)
        {
            var detail = Facade<AuthorFacade>().Get(criteria);

            // log views
            var viewForm = new ViewForm { UserID = UserSession.UserID, SourceID = detail.AuthorID, SourceTable = R.SourceTable.AUTHOR };
            Facade<UserActionFacade>().Viewing(viewForm);

            return View(detail);
        }

        public ActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public JsonResult Form(AuthorForm form, HttpPostedFileBase image, string imagePath)
        {
            if (image != null && image.ContentLength > 0)
            {
                var driveService = GoogleDriveService.GetDriveService();
                var fileID = GoogleDriveService.uploadFile(driveService, image.InputStream, image.FileName, imagePath);
                form.ImageUrl = fileID;
            }
            return SaveChanges(form);
        }

        public ActionResult InlineEdit(InlineEditForm<AuthorDetail> form)
        {
            form.Model = Facade<AuthorFacade>().Get(new AuthorCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}