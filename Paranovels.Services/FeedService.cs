using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class FeedService : BaseService
    {
        public FeedService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(GenericForm<Feed> form)
        {
            var tFeed = Table<Feed>();

            var feed = tFeed.GetOrAdd(w => w.ID == form.DataModel.ID);
            MapProperty(form.DataModel, feed, form.InlineEditProperty);
            UpdateAuditFields(feed, form.ByUserID);
            // save
            SaveChanges();

            return feed.ID;
        }
    }
}
