using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Paranovels.Facade;
using Paranovels.ViewModels;
using Thi.Web;

namespace Paranovels.Mvc.Controllers
{
    public class TranslatorController : SiteController
    {
        public RedirectResult Index()
        {
            return RedirectPermanent("/");
        }

        public JsonResult Translate(TranslatorCriteria criteria)
        {
            var translated = Facade<TranslatorFacade>().Translate(criteria);
            return Json(translated, JsonRequestBehavior.AllowGet);
        }

        public ContentResult Dictionary(TranslatorCriteria criteria)
        {
            var html = "<div>Implementing</div>";
            if (criteria.From == "zh")
            {
                var url = string.Format(@"http://www.mdbg.net/chindict/chindict_ajax.php?c=wordvocab&i={0}&st=0",
                    HttpUtility.UrlEncode(criteria.Text));
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    html = webClient.DownloadString(url);
                }
                var startIndex = html.IndexOf("<table", StringComparison.OrdinalIgnoreCase);
                var endIndex = html.IndexOf("</body>", StringComparison.OrdinalIgnoreCase);
                if (endIndex > startIndex)
                    return new ContentResult { Content = html.Substring(startIndex, endIndex - startIndex), ContentEncoding = new UTF8Encoding(), ContentType = "text/html" };
            }
            return new ContentResult { Content = html };
        }

        public JsonResult ApplyGlossary(TranslatorCriteria criteria)
        {
            var searchModel = CreateSearchModel(new GlossaryCriteria { ID = criteria.ID, SearchType = criteria.SearchType });
            var glossaries = Facade<SearchFacade>().Search(searchModel);
            foreach (var glossary in glossaries.Data)
            {
                criteria.Text = criteria.Text.Replace(glossary.Raw, glossary.Final);
            }
            return Json(criteria.Text, JsonRequestBehavior.AllowGet);
        }
    }
}