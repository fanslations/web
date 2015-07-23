﻿using System;
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
    public class SeriesFacade : IFacade
    {
        public int AddTranslationScene(SeriesForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new SeriesService(uow);
                var id = service.SaveChanges(form);

                var connectorService = new ConnectorService(uow);

                if (form.Categories != null || form.InlineEditProperty == "Categories")
                {
                    form.Categories = form.Categories ?? new List<int>();
                    var connectorForm = new GenericForm<Connector>
                    {
                        ByUserID = form.ByUserID,
                        DataModel = new Connector { ConnectorType = R.ConnectorType.SERIES_TAGCATEGORY, SourceID = id }
                    };
                    connectorService.Connect(connectorForm, form.Categories);
                }
                if (form.Genres != null || form.InlineEditProperty == "Genres")
                {
                    form.Genres = form.Genres ?? new List<int>();
                    var connectorForm = new GenericForm<Connector>
                    {
                        ByUserID = form.ByUserID,
                        DataModel = new Connector { ConnectorType = R.ConnectorType.SERIES_TAGGENRE, SourceID = id }
                    };
                    connectorService.Connect(connectorForm, form.Genres);
                }
                if (form.Contains != null || form.InlineEditProperty == "Contains")
                {
                    form.Contains = form.Contains ?? new List<int>();
                    var connectorForm = new GenericForm<Connector>
                    {
                        ByUserID = form.ByUserID,
                        DataModel = new Connector { ConnectorType = R.ConnectorType.SERIES_TAGCONTAIN, SourceID = id }
                    };
                    connectorService.Connect(connectorForm, form.Contains);
                }

                return id;
            }
        }

        public SeriesDetail GetTranslationScene(SeriesCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new SeriesService(uow);
                var detail = service.Get(criteria);

                var connectorTypes = new[] { R.ConnectorType.SERIES_TAGCATEGORY, R.ConnectorType.SERIES_TAGGENRE, R.ConnectorType.SERIES_TAGCONTAIN };
                var connectors = service.View<Connector>()
                                                 .Where(w => w.IsDeleted == false && connectorTypes.Contains(w.ConnectorType) && w.SourceID == detail.SeriesID).ToList();

                var tagTypes = new[] { R.TagType.NOVEL_CATEGORY, R.TagType.NOVEL_GENRE, R.TagType.NOVEL_CONTAIN };
                var tags = service.View<Tag>().Where(w => tagTypes.Contains(w.TagType)).ToList();

                detail.Categories = tags.Where(w => w.TagType == R.TagType.NOVEL_CATEGORY && connectors.Any(a => a.TargetID == w.TagID)).ToList();

                detail.Genres = tags.Where(w => w.TagType == R.TagType.NOVEL_GENRE && connectors.Any(a => a.TargetID == w.TagID)).ToList();

                detail.Contains = tags.Where(w => w.TagType == R.TagType.NOVEL_CONTAIN && connectors.Any(a => a.TargetID == w.TagID)).ToList();

                if (detail.GroupID > 0)
                {
                    detail.Group = service.View<Group>().Where(w => w.GroupID == detail.GroupID).SingleOrDefault() ?? new Group();
                }

                // get data for user lists
                detail.UserLists = service.View<UserList>().Where(w => w.IsDeleted == false && w.UserID == criteria.ByUserID).ToList();
                var userListIDs = detail.UserLists.Select(s => s.UserListID).ToList();
                detail.UserListConnector = service.View<Connector>().Where(w=> w.ConnectorType == R.ConnectorType.SERIES_USERLIST && w.SourceID == detail.ID && userListIDs.Contains(w.TargetID)).SingleOrDefault();

                detail.Releases = service.View<Release>().Where(w => w.SeriesID == detail.SeriesID).ToList();
                return detail;
            }
        }

        public ReleaseDetail GetRelease(ReleaseCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ReleaseService(uow);
                var detail = service.Get(criteria);
                
                detail.Series = service.View<Series>().Where(w => w.SeriesID == detail.SeriesID).SingleOrDefault() ?? new Series();

                detail.Group = service.View<Group>().Where(w => w.GroupID == detail.GroupID).SingleOrDefault() ?? new Group();

                detail.Summarize = service.View<Summarize>().Where(w=> w.SourceTable == R.SourceTable.RELEASE && w.SourceID == detail.ReleaseID).SingleOrDefault() ?? new Summarize();

                return detail;
            }
        }

        public int AddRelease(ReleaseForm releaseForm)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ReleaseService(uow);
                return service.SaveChanges(releaseForm);
            }
        }
    }
}