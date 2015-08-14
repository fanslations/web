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
    public class AuthorService : BaseService
    {
        public AuthorService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(AuthorForm form)
        {
            var tAuthor = Table<Author>();

            var author = tAuthor.GetOrAdd(w => w.ID == form.ID);
            MapProperty(form, author, form.InlineEditProperty);
            UpdateAuditFields(author, form.ByUserID);
            // save
            SaveChanges();

            return author.ID;
        }

        public AuthorDetail Get(AuthorCriteria criteria)
        {
            var qAuthor = View<Author>().All();

            if (criteria.IDToInt > 0)
            {
                qAuthor = qAuthor.Where(w => w.ID == criteria.IDToInt);
            }

            var author = qAuthor.SingleOrDefault();
            if (author == null) return null;

            var detail = new AuthorDetail();
            MapProperty(author, detail);

            return detail;
        }

        public PagedList<AuthorGrid> Search(SearchModel<AuthorCriteria> searchModel)
        {
            var qAuthor = View<Author>().All();
            var qSummarize = View<Summarize>().Where(w => w.SourceTable == R.SourceTable.AUTHOR);
            var qAka = View<Aka>().Where(w => w.SourceTable == R.SourceTable.AUTHOR);

            var c = searchModel.Criteria;

            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                var model = new Author();
                var columns = new[]
                {
                    model.PropertyName(m => m.Name),
                };
                qAuthor = qAuthor.Search(columns, c.Query.ToSearchKeywords()) as IQueryable<Author>;
            }
            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                qAuthor = new AkaService(_uow).Union(qAuthor, c);
            }

            var results = qAuthor.GroupJoin(qSummarize, r => r.ID, s => s.SourceID,
                (r, s) => new {Author = r, Summarize = s.DefaultIfEmpty()})
                .SelectMany(sm => sm.Summarize.Select(s => new AuthorGrid
                {
                    ID = sm.Author.ID,
                    UpdatedDate = sm.Author.UpdatedDate,
                    Name = sm.Author.Name,
                    Url = sm.Author.Url,
                    ImageUrl = sm.Author.ImageUrl,

                    CommentCount = s.CommentCount,
                    ViewCount = s.ViewCount,
                    VoteUp = s.VoteUp,
                    VoteDown = s.VoteDown,
                    QualityCount = s.QualityCount,
                    QualityScore = s.QualityScore

                }));

            // apply sort
            //results = results.Sort(c);

            return results.ToPagedList(searchModel.PagedListConfig);
        }
    }
}
