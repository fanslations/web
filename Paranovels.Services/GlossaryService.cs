using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class GlossaryService : BaseService
    {
        public GlossaryService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(GlossaryForm form)
        {
            var tGlossary = Table<Glossary>();

            var glossary = tGlossary.GetOrAdd(w => w.ID == form.ID);
            MapProperty(form, glossary, form.InlineEditProperty);
            UpdateAuditFields(glossary, form.ByUserID);
            // save
            SaveChanges();

            return glossary.ID;
        }

        public PagedList<GlossaryGrid> Search(SearchModel<GlossaryCriteria> searchModel)
        {
            var qGlossary = View<Glossary>().All();
            var qSummarize = View<Summarize>().Where(w => w.SourceTable == R.SourceTable.RELEASE);

            var c = searchModel.Criteria;

            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                var model = new Glossary();
                var columns = new[]
                {
                    model.PropertyName(m => m.Raw),
                    model.PropertyName(m => m.Final)
                };
                qGlossary = qGlossary.Search(columns, c.Query.ToSearchKeywords()) as IQueryable<Glossary>;
            }

            if (c.IDs != null)
            {
                qGlossary = qGlossary.Where(w => c.IDs.Contains(w.ID));
            }

            if (c.RawLanguages != null)
            {
                qGlossary = qGlossary.Where(w => c.RawLanguages.Contains(w.RawLanguage));

            }

            var results = qGlossary.GroupJoin(qSummarize, g => g.ID, s => s.SourceID,
                (g, s) => new { Glossary = g, Summarize = s.DefaultIfEmpty() })
                .SelectMany(sm => sm.Summarize.Select(s => new GlossaryGrid
                {
                    ID = sm.Glossary.ID,
                    InsertedBy = sm.Glossary.InsertedBy,
                    InsertedDate = sm.Glossary.InsertedDate,
                    UpdatedBy = sm.Glossary.UpdatedBy,
                    UpdatedDate = sm.Glossary.UpdatedDate,
                    IsDeleted = sm.Glossary.IsDeleted,
                    RawLanguage = sm.Glossary.RawLanguage,
                    Raw = sm.Glossary.Raw,
                    Final = sm.Glossary.Final,
                    Definition = sm.Glossary.Definition,
                    Type = sm.Glossary.Type,

                    CommentCount = s.CommentCount,
                    ViewCount = s.ViewCount,
                    VoteUp = s.VoteUp,
                    VoteDown = s.VoteDown,
                    QualityCount = s.QualityCount,
                    QualityScore = s.QualityScore

                }));

            // apply sorted
            results = results.Sort(c);

            return results.ToPagedList(searchModel.PagedListConfig);
        }

        public GlossaryDetail Get(GlossaryCriteria criteria)
        {
            var qGlossary = View<Glossary>().All();

            if (criteria.IDToInt > 0)
            {
                qGlossary = qGlossary.Where(w => w.ID == criteria.IDToInt);
            }

            var release = qGlossary.SingleOrDefault();
            if (release == null) return null;

            var detail = new GlossaryDetail();
            MapProperty(release, detail);

            return detail;
        }
    }
}
