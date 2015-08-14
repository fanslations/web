using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class StickyService : BaseService
    {
        public StickyService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(StickyForm form)
        {
            var tSticky = Table<Sticky>();

            var sticky = tSticky.GetOrAdd(w => w.ID == form.ID ||
                (form.ID == 0 && w.SourceID == form.SourceID && w.SourceTable == form.SourceTable));

            MapProperty(form, sticky, form.InlineEditProperty);
            UpdateAuditFields(sticky, form.ByUserID);
            // save
            SaveChanges();

            return sticky.ID;
        }
    }
}
