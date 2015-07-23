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
        public int Viewing(int userID, int sourceID, int sourceTable)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                return new UserActionService(uow).Viewing(userID, sourceID, sourceTable);
            }
        }

        public int Voting(VoteForm form)
        {
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
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserActionService(uow);
                var id = service.Reading(form);

                //var vote = service.SummarizeRate(form);
                return id;
            }
        }
    }
}
