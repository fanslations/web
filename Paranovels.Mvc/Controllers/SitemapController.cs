using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;
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
                new SitemapNode(Url.Action("Index", "Author")),
                new SitemapNode(Url.Action("Index", "Glossary")),
                new SitemapNode(Url.Action("List", "Tag", new { ID = R.TagType.CATEGORY, Seo = R.TagType.Translate[R.TagType.CATEGORY].ToSeo() })),
                new SitemapNode(Url.Action("List", "Tag", new { ID = R.TagType.GENRE, Seo = R.TagType.Translate[R.TagType.GENRE].ToSeo() })),
            };

            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

        public ActionResult Release()
        {
            var releases = Facade<QueryFacade>().SearchRelease(new ReleaseCriteria());
            var nodes = releases.OrderByDescending(o => o.UpdatedDate).Select(s => new SitemapNode(Url.Action("Detail", "Release", new { ID = s.ID, Seo = s.Title.ToSeo() })));
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

        public ActionResult Series()
        {
            var series = Facade<QueryFacade>().SearchSeries(new SeriesCriteria());
            var nodes = series.OrderByDescending(o=>o.UpdatedDate).Select(s => new SitemapNode(Url.Action("Detail","Series", new { ID = s.ID, Seo = s.Title.ToSeo() })));
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

        public ActionResult Novel()
        {
            var series = Facade<QueryFacade>().SearchNovel(new NovelCriteria());
            var nodes = series.OrderByDescending(o => o.UpdatedDate).Select(s => new SitemapNode(Url.Action("Detail", "Novel", new { ID = s.ID, Seo = s.Title.ToSeo() })));
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

        public ActionResult Chapter()
        {
            var series = Facade<QueryFacade>().SearchChapter(new ChapterCriteria());
            var nodes = series.OrderByDescending(o => o.UpdatedDate).Select(s => new SitemapNode(Url.Action("Detail", "Chapter", new { ID = s.ID, Seo = s.Title.ToSeo() })));
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

        public ActionResult Group()
        {
            var groups = Facade<QueryFacade>().SearchGroup(new GroupCriteria());
            var nodes = groups.OrderByDescending(o => o.UpdatedDate).Select(s => new SitemapNode(Url.Action("Detail", "Group", new { ID = s.ID, Seo = s.Name.ToSeo() })));
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

        public ActionResult Author()
        {
            var authors = Facade<QueryFacade>().SearchAuthor(new AuthorCriteria());
            var nodes = authors.OrderByDescending(o => o.UpdatedDate).Select(s => new SitemapNode(Url.Action("Detail", "Author", new { ID = s.ID, Seo = s.Name.ToSeo() })));
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

        public ActionResult Glossary()
        {
            var glossaries = Facade<QueryFacade>().SearchGlossary(new GlossaryCriteria());
            var nodes = glossaries.OrderByDescending(o => o.UpdatedDate).Select(s => new SitemapNode(Url.Action("Detail", "Glossary", new { ID = s.ID, Seo = s.Final.ToSeo() })));
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }
    }
}