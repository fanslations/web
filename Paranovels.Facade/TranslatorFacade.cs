using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.ViewModels;
using Thi.Core;
using Thi.Web;

namespace Paranovels.Facade
{
    public class TranslatorFacade : IFacade
    {
        public string Translate(TranslatorCriteria criteria)
        {
            ApplyGlossary(criteria);

            if (criteria.By == 0)
            {
                return criteria.Text;
            }

            var translator = TranslatorFactory.GetTranslator(criteria.By);

            return translator.Translate(criteria.Text, criteria.From, criteria.To);
        }

        private TranslatorCriteria ApplyGlossary(TranslatorCriteria criteria)
        {
            if (criteria.Glossary == 0) return criteria;

            var searchModel = new SearchModel<GlossaryCriteria>
            {
                Criteria = new GlossaryCriteria { ID = criteria.ID, SearchType = criteria.SearchType },
                PagedListConfig = new PagedListConfig { PageSize = int.MaxValue }
            };
            var glossaries = new SearchFacade().Search(searchModel);
            foreach (var glossary in glossaries.Data)
            {
                criteria.Text = criteria.Text.Replace(glossary.Raw, glossary.Final);
            }
            return criteria;

        }
    }
}
