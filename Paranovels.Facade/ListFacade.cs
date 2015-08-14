using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Thi.Core;
using Paranovels.Services;
using Paranovels.ViewModels;

using Paranovels.DataAccess;

namespace Paranovels.Facade
{
    public class ListFacade : IFacade
    {
        public int AddList(ListForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ListService(uow);
                return service.SaveChanges(form);
            }
        }

        public PagedList<UserList> Search(SearchModel<ListCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ListService(uow);

                return service.Search(searchModel);
            }
        }

        public ListDetail Get(ListCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ListService(uow);

                var detail = service.Get(criteria);

                var listSeriesIDs = service.View<Connector>()
                        .Where(w => w.IsDeleted == false && w.ConnectorType == R.ConnectorType.SERIES_USERLIST)
                        .Where(w => w.TargetID == detail.ID).Select(s => s.SourceID).ToList();

                detail.Series = new SeriesService(uow).Search(new SearchModel<SeriesCriteria>
                {
                    Criteria = new SeriesCriteria { IDs = listSeriesIDs },
                    PagedListConfig = new PagedListConfig { PageSize = int.MaxValue }
                }).Data;

                detail.Releases = service.View<Release>().Where(w => listSeriesIDs.Contains(w.SeriesID)).ToList();

                var releaseIDs = detail.Releases.Select(s => s.ID);
                detail.Reads = service.View<UserRead>().Where(w => w.UserID == criteria.ByUserID)
                    .Where(w => w.SourceTable == R.SourceTable.RELEASE)
                    .Where(w => releaseIDs.Contains(w.SourceID)).ToList();


                return detail;
            }
        }

        public IList<UserList> GetListTemplates()
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                return uow.Repository<UserList>().Where(w => w.IsDeleted == false && w.UserID == 0).ToList();
            }
        }

        public IList<UserList> GetListHasReleases(ListCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var qList = uow.Repository<UserList>().Where(w => w.IsDeleted == false && w.UserID == criteria.ByUserID && w.IsNotifyOfNewRelease);
                var qConnector = uow.Repository<Connector>().Where(w => w.IsDeleted == false && w.ConnectorType == R.ConnectorType.SERIES_USERLIST);
                var qRelease = uow.Repository<Release>().Where(w => w.IsDeleted == false && w.SeriesID > 0);
                var qRead = uow.Repository<UserRead>().Where(w => w.UserID == criteria.ByUserID && w.SourceTable == R.SourceTable.RELEASE);


                var seriesList = qList.Join(qConnector, l => l.ID, c => c.TargetID, (l, c) => new { l.ID, l.Name, SeriesID = c.SourceID });

                var allReleases = qRelease.Join(seriesList, r => r.SeriesID, l => l.SeriesID,
                        (r, l) => new { r.ID,  r.Date, UserListID = l.ID, l.Name, r.SeriesID });

                var readReleases = qRead.Join(allReleases, r => r.SourceID, rl => rl.ID,
                        (r, rl) => rl).GroupBy(g => new { g.SeriesID }).Select(s => new { s.Key.SeriesID, Date = s.Max(m => m.Date) });

                var untouchReleases = allReleases.Where(w => !readReleases.Any(w2=> w2.SeriesID == w.SeriesID))
                        .GroupBy(g => new { g.SeriesID })
                        .Select(s => new { s.Key.SeriesID, Date = DateTime.MinValue });

                // only return releases that has been read once
                var unreadReleases = readReleases.Union(untouchReleases).SelectMany(s => allReleases.Where(w => (w.SeriesID == s.SeriesID && w.Date > s.Date))).ToList();

                return unreadReleases.GroupBy(g => new { g.UserListID, g.Name }).Select(s => new UserList { ID = s.Key.UserListID, Name = s.Key.Name, Type = s.Count(), UpdatedDate = s.Max(m => m.Date) }).ToList();
            }
        }

        public void EmailNotifySubscriber(ReleaseForm release)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var qList = uow.Repository<UserList>().Where(w => w.IsDeleted == false && w.IsEmailNotifyOfNewRelease);
                var qConnector = uow.Repository<Connector>().Where(w => w.IsDeleted == false && w.ConnectorType == R.ConnectorType.SERIES_USERLIST && w.SourceID == release.SeriesID);
                var qUser = uow.Repository<User>().Where(w => w.Email.Contains("@"));

                var lists = qList.Join(qConnector, l => l.ID, c => c.TargetID, (l, c) => l);

                var users = qUser.Join(lists, u => u.ID, l => l.UserID, (u, l) => u).ToList();
                
                if (!users.Any()) return;

                var email = new MailMessage()
                {
                    Subject = string.Format("New Release - {0}", release.Title),
                    Body = string.Format("http://www.fanslations.com/release/detail/{0}/{1}", release.Title.ToSeo(), release.ID)
                };
                // add subscriber to bcc
                foreach (var user in users)
                {
                    email.Bcc.Add(user.Email);
                }

                email.To.Add("fanslations+subscribers@gmail.com");
                EmailService.Instance.Send(email);
            }
        }
    }
}
