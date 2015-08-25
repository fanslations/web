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
        public PagedList<SeriesGrid> Search(SearchModel<SeriesCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new SeriesService(uow);
                var results = service.Search(searchModel);

                var seriesIDs = results.Data.Select(s => s.ID).ToList();
                // latest release
                var releases = service.View<Release>().Where(w => seriesIDs.Contains(w.SeriesID)).ToList();
                // connectors
                var connectors = service.View<Connector>().Where(w => w.IsDeleted == false).Where(w => seriesIDs.Contains(w.SourceID)).ToList();
                // tags
                var tagTypes = new[] { R.TagType.CATEGORY, R.TagType.GENRE };
                var tags = service.View<Tag>().Where(w => w.IsDeleted == false && tagTypes.Contains(w.TagType)).ToList();
                // groups
                var groupIDs = connectors.Where(w => w.ConnectorType == R.ConnectorType.SERIES_GROUP).Select(s => s.TargetID).ToList();
                var groups = service.View<Group>().Where(w => groupIDs.Contains(w.ID)).ToList();
                // user lists 
                var userLists = service.View<UserList>().Where(w => w.IsDeleted == false && w.UserID == searchModel.Criteria.ByUserID).ToList();
                // vote
                var userVotedSeriesIDs = service.View<UserVote>().Where(w => w.SourceTable == R.SourceTable.SERIES && seriesIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { SeriesID = s.SourceID, Vote = s.Vote }).ToList();
                // rate
                var userQualityRatedSeriesIDs = service.View<UserRate>().Where(w => w.SourceTable == R.SourceTable.SERIES && seriesIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { SeriesID = s.SourceID, Rate = s.Rate }).ToList();

                var connectorTypes = new[] {R.ConnectorType.SERIES_TAGCATEGORY, R.ConnectorType.SERIES_TAGGENRE};
                // convert to PagedList<TranslationSceneGrid>
                results.Data = results.Data.Select(s =>
                {
                    s.Releases = releases.Where(w => w.SeriesID == s.ID).ToList();
                    s.Groups = connectors.Where(w => w.ConnectorType == R.ConnectorType.SERIES_GROUP && w.SourceID == s.ID)
                                    .Join(groups, c => c.TargetID, g => g.ID, (c, g) => g).ToList();

                    s.UserLists = userLists;
                    s.Connectors = connectors.Where(w => w.SourceID == s.ID).ToList();
                    s.Tags = tags.Where(w => connectors.Any(w2 => connectorTypes.Contains(w2.ConnectorType) && w2.SourceID == s.ID && w2.TargetID == w.ID)).ToList();

                    s.Voted = userVotedSeriesIDs.Where(w => w.SeriesID == s.ID).Select(s2 => s2.Vote).SingleOrDefault();
                    s.QualityRated = userQualityRatedSeriesIDs.Where(w => w.SeriesID == s.ID).Select(s2 => s2.Rate).SingleOrDefault();

                    return s;
                }).ToList();

                return results;
            }
        }

        public PagedList<NovelGrid> Search(SearchModel<NovelCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new NovelService(uow);
                var results = service.Search(searchModel);

                var novelIDs = results.Data.Select(s => s.ID).ToList();
                // latest chapters
                var chapters = service.View<Chapter>().Where(w => novelIDs.Contains(w.NovelID)).ToList();
                // connectors
                var connectors = service.View<Connector>().Where(w => w.IsDeleted == false).Where(w => novelIDs.Contains(w.SourceID)).ToList();
                // tags
                var tagTypes = new[] { R.TagType.CATEGORY, R.TagType.GENRE };
                var tags = service.View<Tag>().Where(w => w.IsDeleted == false && tagTypes.Contains(w.TagType)).ToList();
                // groups
                var groupIDs = connectors.Where(w => w.ConnectorType == R.ConnectorType.NOVEL_GROUP).Select(s => s.TargetID).ToList();
                var groups = service.View<Group>().Where(w => groupIDs.Contains(w.ID)).ToList();
                // user lists 
                var userLists = service.View<UserList>().Where(w => w.IsDeleted == false && w.UserID == searchModel.Criteria.ByUserID).ToList();
                // vote
                var userVotedSeriesIDs = service.View<UserVote>().Where(w => w.SourceTable == R.SourceTable.NOVEL && novelIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { SeriesID = s.SourceID, Vote = s.Vote }).ToList();
                // rate
                var userQualityRatedSeriesIDs = service.View<UserRate>().Where(w => w.SourceTable == R.SourceTable.NOVEL && novelIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { SeriesID = s.SourceID, Rate = s.Rate }).ToList();

                var connectorTypes = new[] { R.ConnectorType.NOVEL_TAGCATEGORY, R.ConnectorType.NOVEL_TAGGENRE };
                // convert to PagedList<TranslationSceneGrid>
                results.Data = results.Data.Select(s =>
                {
                    s.Chapters = chapters.Where(w => w.NovelID == s.ID).ToList();
                    s.Groups = connectors.Where(w => w.ConnectorType == R.ConnectorType.NOVEL_GROUP && w.SourceID == s.ID)
                                    .Join(groups, c => c.TargetID, g => g.ID, (c, g) => g).ToList();

                    s.UserLists = userLists;
                    s.Connectors = connectors.Where(w => w.SourceID == s.ID).ToList();
                    s.Tags = tags.Where(w => connectors.Any(w2 => connectorTypes.Contains(w2.ConnectorType) && w2.SourceID == s.ID && w2.TargetID == w.ID)).ToList();

                    s.Voted = userVotedSeriesIDs.Where(w => w.SeriesID == s.ID).Select(s2 => s2.Vote).SingleOrDefault();
                    s.QualityRated = userQualityRatedSeriesIDs.Where(w => w.SeriesID == s.ID).Select(s2 => s2.Rate).SingleOrDefault();

                    return s;
                }).ToList();

                return results;
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

        public PagedList<GroupGrid> Search(SearchModel<GroupCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GroupService(uow);
                var results = service.Search(searchModel);

                var groupIDs = results.Data.Select(s => s.ID).Distinct();

                // latest release
                var releases = service.View<Release>().Where(w => groupIDs.Contains(w.GroupID)).ToList();

                var userVotedGroupIDs =
                    service.View<UserVote>().Where(w => w.SourceTable == R.SourceTable.GROUP && groupIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { GroupID = s.SourceID, Vote = s.Vote }).ToList();

                var userQualityRatedGroupIDs =
                    service.View<UserRate>().Where(w => w.SourceTable == R.SourceTable.GROUP && groupIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { GroupID = s.SourceID, Rate = s.Rate }).ToList();

                var data = results.Data.Select(s =>
                {
                    s.Releases = releases.Where(w => w.GroupID == s.ID).ToList();

                    s.Voted = userVotedGroupIDs.Where(w => w.GroupID == s.ID).Select(s2 => s2.Vote).SingleOrDefault();
                    s.QualityRated = userQualityRatedGroupIDs.Where(w => w.GroupID == s.ID).Select(s2 => s2.Rate).SingleOrDefault();

                    return s;
                }).ToList();



                return new PagedList<GroupGrid> { Config = results.Config, Data = data };
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
                var groups = service.View<Group>().Where(w => groupIDs.Contains(w.ID)).ToList();

                // series
                var seriesIDs = pagedList.Data.Select(s => s.SeriesID).Distinct();
                var series = service.View<Series>().Where(w => seriesIDs.Contains(w.ID)).ToList();

                // user lists 
                var userLists = service.View<UserList>().Where(w => w.IsDeleted == false && w.UserID == searchModel.Criteria.ByUserID).ToList();

                var connectors = service.View<Connector>().Where(w => w.IsDeleted == false && seriesIDs.Contains(w.SourceID)).ToList();

                // tag
                var tagTypes = new[] {R.TagType.CATEGORY, R.TagType.GENRE};
                var tags = service.View<Tag>().Where(w => w.IsDeleted == false && tagTypes.Contains(w.TagType)).ToList();

                // user actions
                var releaseIDs = pagedList.Data.Select(s => s.ID).Distinct();
                var userVotedReleaseIDs =
                    service.View<UserVote>().Where(w => w.SourceTable == R.SourceTable.RELEASE && releaseIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { ReleaseID = s.SourceID, Vote = s.Vote }).ToList();

                var userQualityRatedReleaseIDs =
                    service.View<UserRate>().Where(w => w.SourceTable == R.SourceTable.RELEASE && releaseIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { ReleaseID = s.SourceID, Rate = s.Rate }).ToList();

                var userReadReleaseIDs = service.View<UserRead>().Where(w => w.SourceTable == R.SourceTable.RELEASE && releaseIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                        .Select(s => new { ReleaseID = s.SourceID }).ToList();

                // sticky
                var stickies = service.View<Sticky>().Where(w => w.IsDeleted == false && w.SourceTable == R.SourceTable.RELEASE).ToList();
                if (stickies.Any())
                {
                    var stickyPageList = service.Search(new SearchModel<ReleaseCriteria>
                    {
                        Criteria = new ReleaseCriteria { IDs = stickies.Select(s => s.SourceID).ToList() },
                        PagedListConfig = new PagedListConfig { PageSize = int.MaxValue }
                    });
                    var stickyDatas = stickyPageList.Data.Select(s =>
                    {
                        s.IsSticky = true;
                        return s;
                    });
                    pagedList.Data = pagedList.Data.Union(stickyDatas).ToList();
                }

                var connectorTypes = new[] { R.ConnectorType.SERIES_TAGCATEGORY, R.ConnectorType.SERIES_TAGGENRE };
                var data = pagedList.Data.Select(s =>
                {
                    s.Group = groups.SingleOrDefault(w => w.ID == s.GroupID);
                    s.Series = series.SingleOrDefault(w => w.ID == s.SeriesID);
                    s.UserLists = userLists;
                    s.Connectors = connectors;
                    s.Tags = tags.Where(w=> connectors.Any(w2 => connectorTypes.Contains(w2.ConnectorType) && w2.SourceID == s.SeriesID &&w2.TargetID ==w.ID)).ToList();

                    s.Voted = userVotedReleaseIDs.Where(w => w.ReleaseID == s.ID).Select(s2 => s2.Vote).SingleOrDefault();
                    s.QualityRated = userQualityRatedReleaseIDs.Where(w => w.ReleaseID == s.ID).Select(s2 => s2.Rate).SingleOrDefault();
                    s.IsRead = userReadReleaseIDs.Any(w => w.ReleaseID == s.ID);
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
                        g => g.ID, cn => cn.TargetID, (g, cn) => g);

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
                var users = service.View<User>().Where(w => userIDs.Contains(w.ID)).ToList();

                var commentIDs = pagedList.Data.Select(s => s.ID).Distinct();
                var userVotedCommentIDs = service.View<UserVote>().Where(w => w.SourceTable == R.SourceTable.COMMENT && commentIDs.Contains(w.SourceID) && w.UserID == searchModel.Criteria.ByUserID)
                                                 .Select(s => new { UserCommentID = s.SourceID, Vote = s.Vote }).ToList();

                pagedList.Data = pagedList.Data.Select(s =>
                {
                    s.User = users.SingleOrDefault(w => w.ID == s.InsertedBy);

                    s.Voted = userVotedCommentIDs.Where(w => w.UserCommentID == s.ID).Select(s2 => s2.Vote).SingleOrDefault();
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
