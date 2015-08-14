using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class TagService : BaseService
    {
        public TagService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public int SaveChanges(TagForm form)
        {
            var tTag = Table<Tag>();

            var tag = tTag.GetOrAdd(w => w.ID == form.ID || (w.TagType == form.TagType && w.Name == form.Name));
            UpdateAuditFields(tag, form.ByUserID);
            MapProperty(form, tag, form.InlineEditProperty);
            // save
            SaveChanges();

            return tag.ID;
        }

        public PagedList<Tag> Search(SearchModel<TagCriteria> searchModel)
        {
            var qTag = Table<Tag>().All();

            var c = searchModel.Criteria;
            if (c.Types != null)
            {
                qTag = qTag.Where(w => c.Types.Contains(w.TagType));
            }

            return qTag.ToPagedList(searchModel.PagedListConfig);
        }

        public IList<Tag> GetList(GenericCriteria<Tag> criteria)
        {
            var qTag = Table<Tag>().All();
            
            qTag = qTag.Where(w => criteria.DataModel.TagType == w.TagType);

            return qTag.ToList();
        }

        public Tag Get(TagCriteria criteria)
        {
            var qTag = View<Tag>().All();
            return qTag.SingleOrDefault(w => w.ID == criteria.IDToInt);
        }
    }

}
