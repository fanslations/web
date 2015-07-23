using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            var c = searchModel.Criteria;

            if (c.SourceTable > 0 && c.SourceID > 0)
            {
                qComment = qComment.Where(w => w.SourceID == c.SourceID && w.SourceTable == c.SourceTable);
            }
            if(c.SourceTable > 0 && c.SourceIDs != null && c.SourceIDs.Any())
            {
                qComment = qComment.Where(w => c.SourceIDs.Contains(w.SourceID) && w.SourceTable == c.SourceTable);
            }

            var results = qComment.Select(s => new CommentGrid
            {
                UserCommentID = s.UserCommentID,
                InsertedBy =  s.InsertedBy,
                InsertedDate = s.InsertedDate,
                UpdatedBy = s.UpdatedBy,
                UpdatedDate = s.UpdatedDate,
                IsDeleted = s.IsDeleted,
                UserID = s.UserID,
                SourceID = s.SourceID,
                SourceTable = s.SourceTable,
                Comment = s.Comment
            });
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
