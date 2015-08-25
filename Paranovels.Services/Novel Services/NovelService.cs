using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class NovelService : BaseService
    {
        public NovelService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }

        public int SaveChanges(NovelForm novelForm)
        {
            var tNovel = Table<Novel>();

            var novel = tNovel.GetOrAdd(w => w.ID == novelForm.ID);
            UpdateAuditFields(novel, novelForm.ByUserID);
            MapProperty(novelForm, novel, novelForm.InlineEditProperty);
            // save
            SaveChanges();

            return novel.ID;
        }

        public NovelDetail Get(NovelCriteria criteria)
        {
            var qNovel = Table<Novel>().All();

            if (criteria.IDToInt > 0)
            {
                qNovel = qNovel.Where(w => w.ID == criteria.IDToInt);
            }
            if (!string.IsNullOrWhiteSpace(criteria.Title))
            {
                qNovel = qNovel.Where(w => w.Title == criteria.Title);
            }
            
            var novel = qNovel.FirstOrDefault();
            if (novel == null) return null;

            var novelDetail = new NovelDetail();
            MapProperty(novel, novelDetail);

            return novelDetail;
        }


        public PagedList<NovelGrid> Search(SearchModel<NovelCriteria> searchModel)
        {
            var qNovel = View<Novel>().All();
            var qSummarize = View<Summarize>().Where(w => w.SourceTable == R.SourceTable.NOVEL);
            var qAka = View<Aka>().Where(w => w.SourceTable == R.SourceTable.NOVEL);

            var c = searchModel.Criteria;

            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                var columns = new[] { "Title" };
                qNovel = qNovel.Search(columns, c.Query.ToSearchKeywords()) as IQueryable<Novel>;
            }
            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                qNovel = new AkaService(_uow).Union(qNovel, c);
            }

            var results = qNovel.GroupJoin(qSummarize, r => r.ID, s => s.SourceID,
                (r, s) => new { Novel = r, Summarize = s.DefaultIfEmpty() })
                .SelectMany(sm => sm.Summarize.Select(s => new NovelGrid
                {
                    ID = sm.Novel.ID,
                    UpdatedDate = sm.Novel.UpdatedDate,
                    Title = sm.Novel.Title,
                    ImageUrl = sm.Novel.ImageUrl,
                    Synopsis = sm.Novel.Synopsis,
                    Status = sm.Novel.Status,
                    RawLanguage = sm.Novel.RawLanguage,

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
