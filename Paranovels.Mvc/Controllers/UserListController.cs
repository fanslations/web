using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public JsonResult Form(ListForm form, string colorHex)
        {
            if (!string.IsNullOrWhiteSpace(colorHex))
            {
                form.Color = colorHex.ToColorInt();
            }
            form.UserID = UserSession.UserID;
            return SaveChanges(form);
        }

        public ActionResult InlineEdit(InlineEditForm<ListDetail> form)
        {
            form.Model = Facade<ListFacade>().Get(new ListCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}