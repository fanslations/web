﻿using System;
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
            var detail = Facade<SeriesFacade>().GetRelease(criteria);

            return View(detail);
        }

        public RedirectResult Out(int id, string url)
        {
            var userID = UserSession.UserID;
            // log views
            Facade<UserActionFacade>().Viewing(userID, id, R.SourceTable.RELEASE);

            return RedirectPermanent(url);
        }

        public RedirectResult Read(int id, string url)
        {
            var userID = UserSession.UserID;
            // mark as read
            Facade<UserActionFacade>().Reading(new ReadForm { ByUserID = userID, UserID = userID, SourceID = id, SourceTable = R.SourceTable.RELEASE });

            return RedirectPermanent(url);
        }


        [HttpPost]
        public JsonResult Form(ReleaseForm form, HttpPostedFileBase image, string imagePath)
        {
            if (!string.IsNullOrWhiteSpace(form.Url))
            {
                form.UrlHash = form.Url.GetIntHash();
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