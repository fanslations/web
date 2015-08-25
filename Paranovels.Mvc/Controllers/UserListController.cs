using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Mvc.Controllers
{
    public class UserListController : SiteController
    {
        public ActionResult Index(ListCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            var pagedList = Facade<ListFacade>().Search(searchModel);

            ViewBag.ListTemplates = Facade<ListFacade>().GetListTemplates();
            return View(pagedList);
        }

        public ActionResult Detail(ListCriteria criteria)
        {
            criteria.ByUserID = UserSession.UserID;
            var detail = Facade<ListFacade>().Get(criteria);

            return View(detail);
        }

        public ActionResult Public(ListCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            criteria.ByUserID = 0;
            criteria.IsPublic = true; // get public list only
            var pagedList = Facade<ListFacade>().Search(searchModel);

            return View(pagedList);
        }

        [HttpPost]
        public JsonResult Form(ListForm form, string colorHex)
        {
            var userID = UserSession.UserID;
            // list exist then only allow user to change settings
            if (form.ID > 0 && form.UserID != userID)
            {
                var result = new
                {
                    IsSuccessful = false,
                    ErrorMessage = "Unauthorized",
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(colorHex))
            {
                form.Color = colorHex.ToColorInt();
            }
            
            return SaveChanges(form);
        }

        public ActionResult InlineEdit(InlineEditForm<ListDetail> form)
        {
            form.Model = Facade<ListFacade>().Get(new ListCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}