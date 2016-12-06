using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Mvc.Controllers
{
    public class TagController : SiteController
    {
        // GET: Tag
        public ActionResult List(TagCriteria criteria)
        {
            criteria.Types = new[] { criteria.IDToInt };
            var searchModel = CreateSearchModel(criteria);
            searchModel.PagedListConfig.PageSize = 999;
            searchModel.PagedListConfig.OrderBy = new Tag().PropertyName(m => m.Name);
            var pagedList = Facade<SearchFacade>().Search(searchModel);
            return View(pagedList);
        }

        public ActionResult Add(TagCriteria criteria)
        {
            return View(new TagForm { TagType = criteria.IDToInt });
        }

        public ActionResult Detail(TagCriteria criteria)
        {
            var detail = Facade<TagFacade>().Get(criteria);
            return View("Detail", detail);
        }

        [HttpPost]
        public JsonResult Form(TagForm form)
        {
            return SaveChanges(form, Facade<TagFacade>().AddTag);
        }

        public ActionResult InlineEdit(InlineEditForm<TagDetail> form)
        {
            form.Model = Facade<TagFacade>().Get(new TagCriteria { ID = form.ID });
            return View("_InlineEditPartial", form);
        }
    }
}