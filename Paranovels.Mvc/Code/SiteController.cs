using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;
using Thi.Web;

namespace Paranovels.Mvc
{
    [Stopwatch]
    public class SiteController : Controller
    {
        protected SearchModel<T> CreateSearchModel<T>(T criteria, string prefix = "") where T : BaseCriteria
        {
            criteria.ByUserID = UserSession.UserID;
            return new SearchModel<T>
            {
                Criteria = criteria,
                PagedListConfig = new PagedListConfig { Prefix = prefix }.ConfigWith(Request.QueryString),
            };
        }

        protected ActionResult FeedGenerator<T>(IEnumerable<T> feedItems, string feedType) where T : class, IFeed, new()
        {
            var feed = new SyndicationFeed();
            var items = new List<SyndicationItem>();

            foreach (var data in feedItems)
            {
                var item = new SyndicationItem();

                item.Id = data.Url;
                item.Title = new TextSyndicationContent(data.Title);
                item.Content = new TextSyndicationContent(data.Content);
                item.BaseUri = new Uri("http://www.fanslations.com");
                item.Links.Add(new SyndicationLink(new Uri(item.BaseUri, data.Url)));
                item.PublishDate = data.InsertedDate;
                item.LastUpdatedTime = data.UpdatedDate;

                items.Add(item);
            }
            feed.Items = items;

            return new FeedResult(feed, feedType);
        }

        protected static string RenderViewToHtml(Controller thisController, string viewName, object model)
        {
            // assign the model of the controller from which this method was called to the instance of the passed controller (a new instance, by the way)
            thisController.ViewData.Model = model;

            // initialize a string builder
            using (var sw = new StringWriter())
            {
                // find and load the view or partial view, pass it through the controller factory
                var viewResult = ViewEngines.Engines.FindPartialView(thisController.ControllerContext, viewName);
                var viewContext = new ViewContext(thisController.ControllerContext, viewResult.View, thisController.ViewData, thisController.TempData, sw);

                // render it
                viewResult.View.Render(viewContext, sw);

                //return the razorized view/partial-view as a string
                return sw.ToString();
            }
        }

        protected T Facade<T>() where T : class, IFacade
        {
            return FacadeFactory.Create<T>();
        }

        public ActionResult AngularViews(string id)
        {
            return View("ng-" + id);
        }

        public JsonResult ViewModels(string id)
        {
            var model = new object();
            var type = Type.GetType(string.Format("Paranovels.ViewModels.{0}, Paranovels.ViewModels, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", id), false, true);
            return Json(type == null ? model : Activator.CreateInstance(type), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataModels(string id)
        {
            var model = new object();
            var type = Type.GetType(string.Format("Paranovels.DataModels.{0}, Paranovels.DataModels, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", id), false, true);
            return Json(type == null ? model : Activator.CreateInstance(type), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveChanges<T>(T model, string callbackUrl = null) where T : class, IFormModel, new()
        {
            // set update by userID
            model.ByUserID = UserSession.UserID;
            var id = Facade<UpdateFacade>().SaveChanges(model);
            var result = new
            {
                IsSuccessful = id > 0,
                RedirectUrl = string.IsNullOrWhiteSpace(callbackUrl) ? Request.QueryString["ReturnUrl"] ?? Request.Form["ReturnUrl"] ?? Url.Action("Detail", new { ID = id, Seo = (Request.Form["seo"] ?? "new").ToSeo() }) : string.Format(callbackUrl, id),
                ErrorMessage = id < 0 ? "Unable to save data." : "",
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected AuthSession UserSession
        {
            get
            {
                return User.GetSession();
            }
        }
    }
}