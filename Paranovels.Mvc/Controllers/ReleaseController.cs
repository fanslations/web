using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Mvc.Controllers
{
    public class ReleaseController : SiteController
    {
        // GET: Release
        public ActionResult Index(ReleaseCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            var pagedList = Facade<SearchFacade>().Search(searchModel);

            return View(pagedList);
        }

        public ActionResult Detail(ReleaseCriteria criteria)
        {
            criteria.ByUserID = UserSession.UserID;
            var detail = Facade<SeriesFacade>().GetRelease(criteria);
            // log views
            var viewForm = new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.RELEASE };
            Facade<UserActionFacade>().Viewing(viewForm);
            return View(detail);
        }

        public ActionResult NextChapter(SeriesCriteria criteria, DateTime? date)
        {
            if (date.HasValue)
            {
                var detail = Facade<SeriesFacade>().GetSeries(criteria);
                var release = detail.Releases.OrderBy(o => o.Date).FirstOrDefault(w => w.Date > date.Value);
                if (release == null)
                    return View(detail);

                return RedirectPermanent(Url.Action("Detail", "Release", new { ID = release.ID, Seo = release.Title.ToSeo() }));
            }
            return null;
        }

        public ActionResult PreviousChapter(SeriesCriteria criteria, DateTime? date)
        {
            if (date.HasValue)
            {
                var detail = Facade<SeriesFacade>().GetSeries(criteria);
                var release = detail.Releases.OrderByDescending(o => o.Date).FirstOrDefault(w => w.Date < date.Value);
                if (release == null)
                    return View(detail);

                return RedirectPermanent(Url.Action("Detail", "Release", new { ID = release.ID, Seo = release.Title.ToSeo() }));
            }
            return null;
        }

        public RedirectResult Out(int id, string url)
        {
            return RedirectPermanent(url);
        }

        public RedirectResult Read(int id, string url, int seriesID = 0, IList<int> listIDs = null)
        {
            var session = UserSession;
            // mark as read
            var readForm = new ReadForm { UserID = session.UserID, SourceID = id, SourceTable = R.SourceTable.RELEASE };
            Facade<UserActionFacade>().Reading(readForm);

            // add series to list
            if (seriesID > 0 && listIDs != null)
            {
                foreach (var listID in listIDs)
                {
                    var form = new ConnectorForm
                    {
                        ByUserID = UserSession.UserID,
                        ConnectorType = R.ConnectorType.SERIES_USERLIST,
                        SourceID = seriesID,
                        TargetID = listID
                    };
                    Facade<ConnectorFacade>().AddConnector(form);
                }
            }

            return RedirectPermanent(url);
        }


        [HttpPost]
        public JsonResult Form(ReleaseForm form, HttpPostedFileBase image, string imagePath)
        {
            if (!string.IsNullOrWhiteSpace(form.Url))
            {
                form.UrlHash = form.Url.GetIntHash();
            }
            // use current datetime if release date is not specified
            if (form.ID == 0 && form.Date == DateTime.MinValue)
            {
                form.Date = DateTime.Now;
            }
            return SaveChanges(form);
        }

        public ActionResult InlineEdit(InlineEditForm<ReleaseDetail> form)
        {
            form.Model = Facade<SeriesFacade>().GetRelease(new ReleaseCriteria() { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}