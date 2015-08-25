using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Facade
{
    public class QueryFacade : IFacade
    {
        public IList<Tag> SearchTag(TagCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new TagService(uow);
                return service.Search(new SearchModel<TagCriteria>
                {
                    Criteria = criteria,
                    PagedListConfig = new PagedListConfig() { PageSize = 999999 }
                }).Data;
            }
        }

        public IList<Group> SearchGroup(GroupCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var qGroup = uow.Repository<Group>().All();

                if (!string.IsNullOrWhiteSpace(criteria.Query))
                {
                    qGroup = qGroup.Where(w => w.Name.Contains(criteria.Query));
                }
                return qGroup.ToList();
            }
        }

        public IList<int> GetVotes(VoteCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var qUserVote = uow.Repository<UserVote>().All();
                var userVotedIDs = qUserVote.Where(w => w.UserID == criteria.ByUserID && w.SourceTable == criteria.SourceTable).Select(s => s.SourceID).ToList();

                return userVotedIDs;
            }
        }

        public IList<Series> SearchSeries(SeriesCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var qSeries = uow.Repository<Series>().All();

                if (!string.IsNullOrWhiteSpace(criteria.Query))
                {
                    qSeries = qSeries.Where(w => w.Title.Contains(criteria.Query));
                }

                return qSeries.ToList();
            }
        }

        public IList<Author> SearchAuthor(AuthorCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var qAuthor = uow.Repository<Author>().All();

                if (!string.IsNullOrWhiteSpace(criteria.Query))
                {
                    qAuthor = qAuthor.Where(w => w.Name.Contains(criteria.Query));
                }

                return qAuthor.ToList();
            }
        }

        public IList<Release> SearchRelease(ReleaseCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var qRelease = uow.Repository<Release>().All();

                if (!string.IsNullOrWhiteSpace(criteria.Query))
                {
                    qRelease = qRelease.Where(w => w.Title.Contains(criteria.Query));
                }

                return qRelease.ToList();
            }
        }

        public IList<Glossary> SearchGlossary(GlossaryCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var qGlossary = uow.Repository<Glossary>().All();

                if (!string.IsNullOrWhiteSpace(criteria.Query))
                {
                    qGlossary = qGlossary.Where(w => w.Raw.Contains(criteria.Query) || w.Final.Contains(criteria.Query));
                }

                return qGlossary.ToList();
            }
        }
    }
}
