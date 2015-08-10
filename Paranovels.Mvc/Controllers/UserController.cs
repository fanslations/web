using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class UserController : SiteController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public new ActionResult Profile(UserCriteria criteria)
        {
            var detail = Facade<UserFacade>().Get(criteria);
            return View(detail);
        }


        [HttpPost]
        public JsonResult Form(UserForm form, HttpPostedFileBase image, string imagePath)
        {
            return SaveChanges(form);
        }

        public ActionResult InlineEdit(InlineEditForm<UserDetail> form)
        {
            form.Model = Facade<UserFacade>().Get(new UserCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}