using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc.Controllers
{
    public class UserPreferenceController : SiteController
    {
        // GET: UserPreference
        public ActionResult Index(UserCriteria criteria)
        {
            criteria.ID = UserSession.UserID;

            var detail = Facade<UserFacade>().Get(criteria);
            return View(detail);
        }

        [HttpPost]
        public JsonResult Form(PreferenceForm form)
        {
            form.UserID = UserSession.UserID;
            return SaveChanges(form, Facade<UserFacade>().AddPreference);
        }
    }
}