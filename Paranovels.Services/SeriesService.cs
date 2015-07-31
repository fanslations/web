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
    public class SeriesService : BaseService
    {
        public SeriesService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(SeriesForm form)
        {
            var tTranslationScene = Table<Series>();

            var translationScene = tTranslationScene.GetOrAdd(w => w.SeriesID == form.SeriesID);
            UpdateAuditFields(translationScene, form.ByUserID);
            MapProperty(form, translationScene, form.InlineEditProperty);
            // save
            SaveChanges();

            return translationScene.SeriesID;
        }

        public SeriesDetail Get(SeriesCriteria criteria)
        {
            var qTranslationScene = View<Series>().All();

            if (criteria.IDToInt > 0)
            {
                qTranslationScene = qTranslationScene.Where(w => w.SeriesID == criteria.IDToInt);
            }

            var translationScene = qTranslationScene.SingleOrDefault();
            if (translationScene == null) return null;

            var translationSceneDetail = new SeriesDetail();
            MapProperty(translationScene, translationSceneDetail);

            return translationSceneDetail;
        }

        public PagedList<SeriesGrid> Search(SearchModel<SeriesCriteria> searchModel)
        {
            var qSeries = View<Series>().All();
            var qSummarize = View<Summarize>().Where(w => w.SourceTable == R.SourceTable.SERIES);

            var c = searchModel.Criteria;

            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                var columns = new[] { "Title", "Synopsis" };
                qSeries = qSeries.Search(columns, c.Query.ToSearchKeywords()) as IQueryable<Series>;
            }

            if (c.IDs != null)
            {
                qSeries = qSeries.Where(w => c.IDs.Contains(w.SeriesID));
            }



            var results = qSeries.GroupJoin(qSummarize, r => r.SeriesID, s => s.SourceID,
                (r, s) => new { Series = r, Summarize = s.DefaultIfEmpty() })
                .SelectMany(sm => sm.Summarize.Select(s => new SeriesGrid
                {
                    SeriesID = sm.Series.SeriesID,
                    UpdatedDate = sm.Series.UpdatedDate,
                    Title = sm.Series.Title,
                    ImageUrl = sm.Series.ImageUrl,
                    Synopsis = sm.Series.Synopsis,
                    Status = sm.Series.Status,
                    GroupID = sm.Series.GroupID,
                    

                    CommentCount = s.CommentCount,
                    ViewCount = s.ViewCount,
                    VoteUp = s.VoteUp,
                    VoteDown = s.VoteDown,
                    QualityCount = s.QualityCount,
                    QualityScore = s.QualityScore

                }));
            // apply sort
            results = results.Sort(c);

            return results.ToPagedList(searchModel.PagedListConfig);
        }
    }
}
