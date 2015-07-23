using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleMvcSitemap;


namespace Paranovels.Mvc.Controllers
{
    public class SitemapController : SiteController
    {
        // GET: Sitemap
        public ActionResult Index()
        {
            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("Index", "Release")),
                //new SitemapNode(Url.Action("Index", "Novel")),
                new SitemapNode(Url.Action("Index", "NovelTracker")),
                new SitemapNode(Url.Action("Index", "Group")),
                
            };

            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }
    }
}