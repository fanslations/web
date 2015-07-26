using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;
using Thi.Web;

namespace Paranovels.Facade
{
    public class SearchFacade : IFacade
    {
        public PagedList<SeriesGrid> Search(SearchModel<NovelTrackerCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new SeriesService(uow);
                var results = service.Search(searchModel);

                var seriesIDs = results.Data.Select(s => s.SeriesID).ToList();
                // latest release
                var releases = service.View<Release>().Where(w => seriesIDs.Contains(w.SeriesID));
                // translatios group
                var groupIDs = results.Data.Select(s => s.GroupID).ToList();
                var groups = service.View<Group>().Where(w => groupIDs.Contains(w.GroupID)).ToList();

                // user read
                var userReadReleases = service.View<UserRead>().Where(w => w.UserID == searchModel.Criteria.ByUserID)
                    .Where(w => w.SourceTable == R.SourceTable.RELEASE)
                    .Join(releases, ur => ur.SourceID, r => r.ReleaseID, (ur, r) => r).ToList();

                // convert to PagedList<TranslationSceneGrid>
                var data = results.Data.Select(s =>
                {
                    s.Release = releases.OrderByDescending(o => o.Date).FirstOrDefault(w => w.SeriesID == s.SeriesID);
                    s.Group = groups.SingleOrDefault(g => g.GroupID == s.GroupID);

                    s.UserRead = userReadReleases.OrderByDescending(o => o.Date).FirstOrDefault(w => w.SeriesID == s.SeriesID);

                    return s;
                }).ToList();

                return new PagedList<SeriesGrid> { Config = results.Config, Data = data };
            }
        }

        public PagedList<Novel> Search(SearchModel<NovelCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new NovelService(uow);
                return service.Search(searchModel);
            }
        }

        public PagedList<Tag> Search(SearchModel<TagCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new TagService(uow);
                return service.Search(searchModel);
            }
        }

        public PagedList<Group> Search(SearchModel<GroupCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GroupService(uow);
                return service.Search(searchModel);
            }
        }

        public PagedList<ReleaseGrid> Search(SearchModel<ReleaseCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ReleaseService(uow);
                var pagedList = service.Search(searchModel);

                // groups
                var groupIDs = pagedList.Data.Select(s => s.GroupID).Distinct();
                var groups = service.View<Group>().Where(w => groupIDs.Contains(w.GroupID)).ToList();

                // series
                var seriesIDs = pagedList.Data.Select(s => s.SeriesID).Distinct();
                var series = service.View<Series>().Where(w => seriesIDs.Contains(w.SeriesID)).ToList();

                var releaseIDs = pagedList.Data.Select(s => s.ReleaseID).Distinct();
                var userVotedReleaseIDs =
                    service.View<UserVote>().Where(w => w.SourceTable == R.SourceTable.RELEASE && releaseIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { ReleaseID = s.SourceID, Vote = s.Vote }).ToList();

                var userQualityRatedReleaseIDs =
                    service.View<UserRate>().Where(w => w.SourceTable == R.SourceTable.RELEASE && releaseIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { ReleaseID = s.SourceID, Rate = s.Rate }).ToList();

                var userReadReleaseIDs = service.View<UserRead>().Where(w => w.SourceTable == R.SourceTable.RELEASE && releaseIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { ReleaseID = s.SourceID }).ToList();

                var data = pagedList.Data.Select(s =>
                {
                    s.Group = groups.SingleOrDefault(w => w.GroupID == s.GroupID);
                    s.Series = series.SingleOrDefault(w => w.SeriesID == s.SeriesID);

                    s.Voted = userVotedReleaseIDs.Where(w => w.ReleaseID == s.ReleaseID).Select(s2 => s2.Vote).SingleOrDefault();
                    s.QualityRated = userQualityRatedReleaseIDs.Where(w => w.ReleaseID == s.ReleaseID).Select(s2 => s2.Rate).SingleOrDefault();
                    s.IsRead = userReadReleaseIDs.Any(w => w.ReleaseID == s.ReleaseID);
                    return s;
                }).ToList();

                return new PagedList<ReleaseGrid> { Config = pagedList.Config, Data = data };
            }
        }

        public PagedList<Glossary> Search(SearchModel<GlossaryCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GlossaryService(uow);

                var c = searchModel.Criteria;
                var results = service.View<Glossary>().Where(w => w.IsDeleted == false)
                    .Join(service.View<Connector>().Where(w => w.ConnectorType == R.ConnectorType.NOVEL_GLOSSARY && w.SourceID == c.IDToInt),
                        g => g.GlossaryID, cn => cn.TargetID, (g, cn) => g);

                return results.ToPagedList(searchModel.PagedListConfig);
            }
        }

        public PagedList<CommentGrid> Search(SearchModel<CommentCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new CommentService(uow);

                var pagedList = service.Search(searchModel);

                var userIDs = pagedList.Data.Select(s => s.InsertedBy).Distinct();
                var users = service.View<User>().Where(w => userIDs.Contains(w.UserID)).ToList();

                var commentIDs = pagedList.Data.Select(s => s.UserCommentID).Distinct();
                var userVotedCommentIDs = service.View<UserVote>().Where(w => w.SourceTable == R.SourceTable.COMMENT && commentIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                                                 .Select(s => new { UserCommentID = s.SourceID, Vote = s.Vote }).ToList();

                pagedList.Data = pagedList.Data.Select(s =>
                {
                    s.User = users.SingleOrDefault(w => w.UserID == s.InsertedBy);

                    s.Voted = userVotedCommentIDs.Where(w => w.UserCommentID == s.UserCommentID).Select(s2 => s2.Vote).SingleOrDefault();
                    return s;
                }).ToList();


                return pagedList;
            }
        }

        public PagedList<User> Search(SearchModel<UserCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserService(uow);

                var results = service.Search(searchModel);

                return results;
            }
        }
    }
}
