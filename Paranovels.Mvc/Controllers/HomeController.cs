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
            searchModel.PagedListConfig = new PagedListConfig { PageSize = 40 };
            var pagedList = Facade<SearchFacade>().Search(searchModel);

            // alternative version
            if (!string.IsNullOrWhiteSpace(criteria.Alt))
            {
                var feedItems = pagedList.Data.Select(s => new FeedGrid()
                {
                    InsertedDate = s.InsertedDate,
                    UpdatedDate = s.UpdatedDate,
                    Title = s.Title,
                    Url = Url.Action("Detail", "Release", new { ID = s.ID, Seo = s.PrettyTitle.ToSeo() }),
                });
                return FeedGenerator(feedItems, criteria.Alt);
            }

            ViewBag.Releases = pagedList;
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