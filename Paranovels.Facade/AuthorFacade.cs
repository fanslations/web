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
    public class AuthorFacade : IFacade
    {
        public PagedList<AuthorGrid> Search(SearchModel<AuthorCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new AuthorService(uow);
                var results = service.Search(searchModel);

                var authorIDs = results.Data.Select(s => s.ID).ToList();

                var userVotedAuthorIDs = service.View<UserVote>().Where(w => w.SourceTable == R.SourceTable.AUTHOR && authorIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { AuthorID = s.SourceID, Vote = s.Vote }).ToList();
                // rate
                var userQualityRatedAuthorIDs = service.View<UserRate>().Where(w => w.SourceTable == R.SourceTable.AUTHOR && authorIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { AuthorID = s.SourceID, Rate = s.Rate }).ToList();

                var data = results.Data.Select(s =>
                {
                    s.Voted = userVotedAuthorIDs.Where(w => w.AuthorID == s.ID).Select(s2 => s2.Vote).SingleOrDefault();
                    s.QualityRated = userQualityRatedAuthorIDs.Where(w => w.AuthorID == s.ID).Select(s2 => s2.Rate).SingleOrDefault();

                    return s;
                }).ToList();

                return new PagedList<AuthorGrid> { Config = results.Config, Data = data };
            }
        }

        public AuthorDetail Get(AuthorCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new AuthorService(uow);
                var detail = service.Get(criteria);

                var seriesIDs = service.View<Connector>().Where(w => w.IsDeleted == false && w.ConnectorType == R.ConnectorType.SERIES_AUTHOR && w.TargetID == detail.ID).Select(s => s.SourceID).ToList();
                detail.Series = service.View<Series>().Where(w => seriesIDs.Contains(w.ID)).ToList();

                detail.Releases = service.View<Release>().Where(w => seriesIDs.Contains(w.SeriesID)).ToList();

                detail.Summarize = service.View<Summarize>().Where(w => w.SourceTable == R.SourceTable.AUTHOR && w.SourceID == detail.ID).SingleOrDefault() ?? new Summarize();

                detail.UserAction = new UserActionFacade().Get(new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.AUTHOR });

                detail.Akas = service.View<Aka>().Where(w => w.IsDeleted == false && w.SourceID == detail.ID && w.SourceTable == R.SourceTable.AUTHOR).ToList();

                return detail;
            }
        }

        public int AddAuthor(AuthorForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new AuthorService(uow);
                var id = service.SaveChanges(form);

                if (form.Akas != null || form.InlineEditProperty == form.PropertyName(m => m.Akas))
                {
                    var akaService = new AkaService(uow);
                    foreach (var aka in form.Akas)
                    {
                        var akaForm = new AkaForm
                        {
                            ByUserID = form.ByUserID,
                            SourceID = form.ID,
                            SourceTable = R.SourceTable.AUTHOR
                        };
                        new PropertyMapper<Aka, AkaForm>(aka, akaForm).Map();
                        var akaID = akaService.SaveChanges(akaForm);
                    }
                }

                return id;
            }
        }
    }
}
