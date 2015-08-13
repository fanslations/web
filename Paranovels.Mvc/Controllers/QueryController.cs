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
    public class QueryController : SiteController
    {
        // GET: Api
        public JsonResult Genres()
        {
            var tagList = Facade<QueryFacade>().SearchTag(new TagCriteria { Types = new []{ R.TagType.NOVEL_GENRE}});

            return Json(tagList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Categories()
        {
            var tagList = Facade<QueryFacade>().SearchTag(new TagCriteria { Types = new[] { R.TagType.NOVEL_CATEGORY } });

            return Json(tagList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Series(SeriesCriteria criteria)
        {
            var results = Facade<QueryFacade>().SearchSeries(criteria);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Groups(GroupCriteria criteria)
        {
            var groupList = Facade<QueryFacade>().SearchGroup(criteria);
            return Json(groupList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Authors(AuthorCriteria criteria)
        {
            var groupList = Facade<QueryFacade>().SearchAuthor(criteria);
            return Json(groupList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Comments(CommentCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            searchModel.PagedListConfig.PageSize = 99999;
            var pagedList = Facade<SearchFacade>().Search(searchModel);
            return Json(pagedList.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Users(UserCriteria criteria)
        {
            var searchModel = CreateSearchModel(criteria);
            searchModel.PagedListConfig.PageSize = 99999;
            var pagedList = Facade<SearchFacade>().Search(searchModel);
            return Json(pagedList.Data, JsonRequestBehavior.AllowGet);
        }
    }
}