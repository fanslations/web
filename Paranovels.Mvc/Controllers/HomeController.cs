using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Mvc.Controllers
{
    public class HomeController : SiteController
    {
        public ActionResult Index(ReleaseCriteria criteria)
        {
            // get last 100 releases
            var searchModel = CreateSearchModel(criteria);
            searchModel.PagedListConfig = new PagedListConfig { PageSize = 50 };
            ViewBag.Releases = Facade<SearchFacade>().Search(searchModel);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}