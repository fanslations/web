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
    public class CommentService : BaseService
    {
        public CommentService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public int SaveChanges(CommentForm form)
        {
            var tComment = Table<UserComment>();

            var comment = tComment.GetOrAdd(w => w.UserCommentID == form.UserCommentID);
            MapProperty(form, comment, form.InlineEditProperty);
            UpdateAuditFields(comment, form.ByUserID);
            // save
            SaveChanges();

            return comment.UserCommentID;
        }

        public PagedList<CommentGrid> Search(SearchModel<CommentCriteria> searchModel)
        {
            var qComment = Table<UserComment>().All();
            var qSummarize = View<Summarize>().Where(w => w.SourceTable == R.SourceTable.COMMENT);

            var c = searchModel.Criteria;

            if (c.SourceTable > 0 && c.SourceID > 0)
            {
                qComment = qComment.Where(w => w.SourceID == c.SourceID && w.SourceTable == c.SourceTable);
            }
            if(c.SourceTable > 0 && c.SourceIDs != null && c.SourceIDs.Any())
            {
                qComment = qComment.Where(w => c.SourceIDs.Contains(w.SourceID) && w.SourceTable == c.SourceTable);
            }

            var results = qComment.GroupJoin(qSummarize, uc => uc.UserCommentID, s => s.SourceID,
                (uc, s) => new {Comment = uc, Summarize = s.DefaultIfEmpty()})
                .SelectMany(sm => sm.Summarize.Select(s => new CommentGrid
                {
                    UserCommentID = sm.Comment.UserCommentID,
                    InsertedBy = sm.Comment.InsertedBy,
                    InsertedDate = sm.Comment.InsertedDate,
                    UpdatedBy = sm.Comment.UpdatedBy,
                    UpdatedDate = sm.Comment.UpdatedDate,
                    IsDeleted = sm.Comment.IsDeleted,
                    UserID = sm.Comment.UserID,
                    SourceID = sm.Comment.SourceID,
                    SourceTable = sm.Comment.SourceTable,
                    Comment = sm.Comment.Comment,

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


        public CommentDetail Get(CommentCriteria criteria)
        {
            var qUserComment = View<UserComment>().All();
            if (criteria.IDToInt > 0)
            {
                qUserComment = qUserComment.Where(w => w.UserCommentID == criteria.IDToInt);
            }

            var comment = qUserComment.SingleOrDefault();
            if (comment == null) return null;

            var detail = new CommentDetail();
            MapProperty(comment, detail);

            return detail;
        }
    }

}
