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
    public class TagFacade : IFacade
    {

        public int AddTag(TagForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new TagService(uow);
                var id = service.SaveChanges(form);
                return id;
            }
        }

        public TagDetail Get(TagCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new TagService(uow);
                var tag = service.Get(criteria);

                var detail = JsonHelper.Deserialize<TagDetail>(JsonHelper.Serialize(tag));

                if (tag.TagType == R.TagType.GENRE || tag.TagType == R.TagType.CATEGORY || tag.TagType == R.TagType.CONTAIN)
                {
                    var sourceIDs = new[]
                    {
                        R.ConnectorType.SERIES_TAGCATEGORY, 
                        R.ConnectorType.SERIES_TAGGENRE,
                        R.ConnectorType.SERIES_TAGCONTAIN
                    };
                    detail.Series = service.View<Series>().Where(w => w.IsDeleted == false)
                        .Join(service.View<Connector>().Where(w => w.IsDeleted == false).Where(w=> sourceIDs.Contains(w.ConnectorType) && w.TargetID == tag.ID),
                            ts => ts.ID, c => c.SourceID,
                            (ts, c) => ts).ToList();
                }
                return detail;
            }
        }
    }
}
