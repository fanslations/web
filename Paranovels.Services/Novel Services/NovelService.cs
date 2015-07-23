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

            var novel = tNovel.GetOrAdd(w => w.NovelID == novelForm.NovelID);
            UpdateAuditFields(novel, novelForm.ByUserID);
            MapProperty(novelForm, novel, novelForm.InlineEditProperty);
            // save
            SaveChanges();

            return novel.NovelID;
        }

        public NovelDetail Get(NovelCriteria criteria)
        {
            var qNovel = Table<Novel>().All();

            if (criteria.IDToInt > 0)
            {
                qNovel = qNovel.Where(w => w.NovelID == criteria.IDToInt);
            }
            if (!string.IsNullOrWhiteSpace(criteria.Title))
            {
                qNovel = qNovel.Where(w => w.Title == criteria.Title);
            }
            
            var novel = qNovel.FirstOrDefault();
            if (novel == null) return null;

            var novelDetail = new NovelDetail();
            MapProperty(novel, novelDetail);

            // author
            var qTag = Table<Tag>().All();
            var qConnector = Table<Connector>().All();

            var qAuthor = Table<Author>().All();
            var authors = qConnector.Where(w => w.ConnectorType == R.ConnectorType.NOVEL_AUTHOR)
                .Where(w => w.SourceID == novel.NovelID).Select(s => s.TargetID).ToList();
            novelDetail.Authors = qAuthor.Where(w => authors.Contains(w.AuthorID)).ToList();

            // chapter
            var qChapter = Table<Chapter>().All();

            qChapter = qChapter.Where(w => w.NovelID == novel.NovelID);

            novelDetail.Chapters = qChapter.ToList();

            return novelDetail;
        }


        public PagedList<Novel> Search(SearchModel<NovelCriteria> searchModel)
        {
            var qNovel = View<Novel>().All();

            var c = searchModel.Criteria;

            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                var columns = new[] { "Title", "Synopsis" };
                qNovel =
                    qNovel.Search(columns, c.Query.ToSearchKeywords()) as IQueryable<Novel>;
            }

            return qNovel.ToPagedList(searchModel.PagedListConfig);
        }
    }
}
