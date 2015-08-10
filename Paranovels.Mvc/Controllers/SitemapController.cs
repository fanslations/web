using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Paranovels.Common;
using SimpleMvcSitemap;
using Thi.Core;


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
                new SitemapNode(Url.Action("Index", "Project")),
                new SitemapNode(Url.Action("Index", "Release")),
                new SitemapNode(Url.Action("Index", "Series")),
                new SitemapNode(Url.Action("Index", "Group")),
                new SitemapNode(Url.Action("List", "Tag", new { ID = R.TagType.NOVEL_CATEGORY, Seo = R.TagType.Translate[R.TagType.NOVEL_CATEGORY].ToSeo() })),
                new SitemapNode(Url.Action("List", "Tag", new { ID = R.TagType.NOVEL_GENRE, Seo = R.TagType.Translate[R.TagType.NOVEL_GENRE].ToSeo() })),
            };

            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }
    }
}