using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class ReleaseService : BaseService
    {
        public ReleaseService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(ReleaseForm form)
        {
            var tRelease = Table<Release>();

            var release = tRelease.GetOrAdd(w => w.ID == form.ID || (form.ID == 0 && w.UrlHash == form.UrlHash));
            MapProperty(form, release, form.InlineEditProperty);
            UpdateAuditFields(release, form.ByUserID);

            // save
            SaveChanges();

            return release.ID;
        }

        public ReleaseDetail Get(ReleaseCriteria criteria)
        {
            var qRelease = View<Release>().All();

            if (criteria.IDToInt > 0)
            {
                qRelease = qRelease.Where(w => w.ID == criteria.IDToInt);
            }

            var release = qRelease.SingleOrDefault();
            if (release == null) return null;

            var releaseDetail = new ReleaseDetail();
            MapProperty(release, releaseDetail);

            return releaseDetail;
        }

        public PagedList<ReleaseGrid> Search(SearchModel<ReleaseCriteria> searchModel)
        {
            var qRelease = View<Release>().All();
            var qSummarize = View<Summarize>().Where(w => w.SourceTable == R.SourceTable.RELEASE);

            var c = searchModel.Criteria;

            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                var model = new Release();
                var columns = new[]
                {
                    model.PropertyName(m=>m.Title)
                };
                qRelease = qRelease.Search(columns, c.Query.ToSearchKeywords()) as IQueryable<Release>;
            }
            if (c.IDs != null)
            {
                qRelease = qRelease.Where(w => c.IDs.Contains(w.ID));
            }

            //if (c.ByUserID > 0)
            //{
            //    var connectorTypes = new[] {R.ConnectorType.SERIES_TAGCATEGORY, R.ConnectorType.SERIES_TAGGENRE};
            //    var preferenceTypes = new[] {R.PreferenceType.CATEGORY, R.PreferenceType.GENRE};
            //    var qConnector = View<Connector>().Where(w => connectorTypes.Contains(w.ConnectorType) );
            //    var qUserPreference = View<UserPreference>().Where(w => w.UserID == c.ByUserID && preferenceTypes.Contains(w.Type) && w.SourceTable == R.SourceTable.TAG);

            //    var q = qConnector.Join(qUserPreference, cn => cn.TargetID, up => up.SourceID,
            //        (cn, up) => new {cn.SourceID, up.Score})
            //        .GroupBy(g => new {g.SourceID, g.Score})
            //        .Where(w => w.Sum(s => s.Score) < 0)
            //        .Select(s => s.Key.SourceID);

            //    var excludedSeriesIDs = q.ToList();

            //    qRelease = qRelease.Where(w => excludedSeriesIDs.All(a => a != w.SeriesID));
            //}


            var results = qRelease.GroupJoin(qSummarize, r => r.ID, s => s.SourceID,
                (r, s) => new { Release = r, Summarize = s.DefaultIfEmpty() })
                .SelectMany(sm => sm.Summarize.Select(s => new ReleaseGrid
                {
                    ID = sm.Release.ID,
                    InsertedBy = sm.Release.InsertedBy,
                    InsertedDate = sm.Release.InsertedDate,
                    UpdatedBy = sm.Release.UpdatedBy,
                    UpdatedDate = sm.Release.UpdatedDate,
                    IsDeleted = sm.Release.IsDeleted,
                    Date = sm.Release.Date,
                    Title = sm.Release.Title,
                    Url = sm.Release.Url,
                    Type = sm.Release.Type,
                    Summary = sm.Release.Summary,
                    GroupID = sm.Release.GroupID,
                    SeriesID = sm.Release.SeriesID,

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
    }
}
