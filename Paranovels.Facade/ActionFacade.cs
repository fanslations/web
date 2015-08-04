using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Facade
{
    public class UserActionFacade : IFacade
    {
        public UserActionDetail Get(ViewForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserActionService(uow);


                var userAction = new UserActionDetail();
                userAction.Voted =
                    service.View<UserVote>().Where(w => w.SourceTable == form.SourceTable && w.SourceID == form.SourceID && w.UserID == form.UserID).Select(s => s.Vote).SingleOrDefault();

                userAction.QualityRated =
                    service.View<UserRate>().Where(w => w.SourceTable == form.SourceTable && w.SourceID == form.SourceID && w.UserID == form.UserID).Select(s => s.Rate).SingleOrDefault();

                userAction.IsRead = service.View<UserRead>().Where(w => w.SourceTable == form.SourceTable && w.SourceID == form.SourceID && w.UserID == form.UserID).Any();

                return userAction;
            }
        }
        public int Viewing(ViewForm form)
        {
            form.ByUserID = form.UserID; // byUserID has to be the UserID
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserActionService(uow);
                var id = service.Viewing(form);

                var view = service.SummarizeView(form);
                return view;
            }
        }

        public int Voting(VoteForm form)
        {
            form.ByUserID = form.UserID; // byUserID has to be the UserID
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserActionService(uow);
                var id = service.Voting(form);

                var vote = service.SummarizeVote(form);
                return vote;
            }
        }

        public double Rating(RateForm form)
        {
            form.ByUserID = form.UserID; // byUserID has to be the UserID
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserActionService(uow);
                var id = service.Rating(form);

                var vote = service.SummarizeRate(form);
                return vote;
            }
        }

        public int Reading(ReadForm form)
        {
            form.ByUserID = form.UserID; // byUserID has to be the UserID
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserActionService(uow);
                var id = service.Reading(form);

                var read = service.SummarizeRead(form);
                return read;
            }
        }
    }
}
