using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thi.Core;
using Paranovels.Services;
using Paranovels.ViewModels;

using Paranovels.DataAccess;

namespace Paranovels.Facade
{
    public class CommentFacade : IFacade
    {
        public CommentDetail Get(CommentCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new CommentService(uow);
                return service.Get(criteria);
            }
        }

        public int AddComment(CommentForm commentForm)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new CommentService(uow);
                var id = service.SaveChanges(commentForm);

                // new comment, increase comment count
                if (commentForm.ID == 0)
                {
                    var userActionService = new UserActionService(uow);
                    userActionService.SummarizeComment(commentForm);
                }

                return id;
            }
        }
    }
}
