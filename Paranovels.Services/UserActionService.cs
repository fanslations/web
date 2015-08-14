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
    public class UserActionService : BaseService
    {
        public UserActionService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int Reading(ReadForm form)
        {
            var tUserRead = Table<UserRead>();

            var userRead = tUserRead.GetOrAdd(w => w.UserID == form.UserID && w.SourceID == form.SourceID && w.SourceTable == form.SourceTable);
            MapProperty(form, userRead);
            UpdateAuditFields(userRead, form.ByUserID);

            // save
            SaveChanges();

            return userRead.ID;
        }

        public int Viewing(ViewForm form)
        {
            var tUserView = Table<UserView>();

            const int VIEW_COUNT_MINUTES = 5;
            var lastView = DateTime.Now.AddMinutes(5 * -1);

            var userView = tUserView.GetOrAdd(w => w.UserID == form.UserID && w.SourceID == form.SourceID && w.SourceTable == form.SourceTable && w.UpdatedDate > lastView);
            MapProperty(form, userView);
            UpdateAuditFields(userView, form.ByUserID);
            // save
            SaveChanges();

            return userView.UserID;
        }

        public int Voting(VoteForm form)
        {
            var tUserVote = Table<UserVote>();

            var userVote = tUserVote.GetOrAdd(w => w.UserID == form.UserID && w.SourceID == form.SourceID && w.SourceTable == form.SourceTable);
            MapProperty(form, userVote);
            UpdateAuditFields(userVote, form.ByUserID);

            userVote.Vote = form.Vote; // update when Vote is 0
            // save
            SaveChanges();

            return userVote.ID;
        }

        public int SummarizeVote(VoteForm form)
        {
            var tUserVote = View<UserVote>();
            var tSummarize = Table<Summarize>();

            var summarize = tSummarize.GetOrAdd(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable);
            MapProperty(form, summarize);
            UpdateAuditFields(summarize, form.ByUserID);
            summarize.VoteUp = tUserVote.Where(w=> w.SourceID == form.SourceID && w.SourceTable == form.SourceTable).Count(s=> s.Vote == 1);
            summarize.VoteDown = tUserVote.Where(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable).Count(s => s.Vote == -1);
            // save
            SaveChanges();

            return summarize.VoteUp - summarize.VoteDown;
        }

        public int Rating(RateForm form)
        {
            var tUserRate = Table<UserRate>();

            var userVote = tUserRate.GetOrAdd(w => w.UserID == form.UserID && w.SourceID == form.SourceID && w.SourceTable == form.SourceTable);
            MapProperty(form, userVote);
            UpdateAuditFields(userVote, form.ByUserID);

            userVote.Rate = form.Rate; // update when Vote is 0
            // save
            SaveChanges();

            return userVote.ID;
        }

        public double SummarizeRate(RateForm form)
        {
            var tUserRate = View<UserRate>();
            var tSummarize = Table<Summarize>();

            var summarize = tSummarize.GetOrAdd(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable);
            MapProperty(form, summarize);
            UpdateAuditFields(summarize, form.ByUserID);
            summarize.QualityScore = tUserRate.Where(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable).Sum(s => s.Rate);
            summarize.QualityCount = tUserRate.Where(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable).Count();
            // save
            SaveChanges();

            return ((double)summarize.QualityScore / summarize.QualityCount);
        }

        public int SummarizeComment(CommentForm form)
        {
            var tComment = View<UserComment>();
            var tSummarize = Table<Summarize>();

            var summarize = tSummarize.GetOrAdd(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable);
            MapProperty(form, summarize);
            UpdateAuditFields(summarize, form.ByUserID);
            summarize.CommentCount = summarize.CommentCount + 1;

            // save
            SaveChanges();

            // walk up the chain and increase comment count
            if (form.SourceTable == R.SourceTable.COMMENT)
            {
                var commentForm = tComment.Where(w => w.ID == form.SourceID).Select(s => new CommentForm
                {
                    SourceID = s.SourceID,
                    SourceTable = s.SourceTable
                }).SingleOrDefault();

                SummarizeComment(commentForm);
            }

            return summarize.CommentCount;
        }

        public int SummarizeRead(ReadForm form)
        {
            var tUserRead = View<UserRead>();
            var tSummarize = Table<Summarize>();

            var summarize = tSummarize.GetOrAdd(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable);
            MapProperty(form, summarize);
            UpdateAuditFields(summarize, form.ByUserID);
            summarize.ReadCount = tUserRead.Where(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable).Count();
            // save
            SaveChanges();

            return summarize.ReadCount;
        }

        public int SummarizeView(ViewForm form)
        {
            var tUserView = View<UserView>();
            var tSummarize = Table<Summarize>();

            var summarize = tSummarize.GetOrAdd(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable);
            MapProperty(form, summarize);
            UpdateAuditFields(summarize, form.ByUserID);
            summarize.ViewCount = tUserView.Where(w => w.SourceID == form.SourceID && w.SourceTable == form.SourceTable).Count();
            // save
            SaveChanges();

            return summarize.ViewCount;
        }
    }
}
