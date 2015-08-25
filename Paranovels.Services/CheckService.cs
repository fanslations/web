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
    public class CheckService : BaseService
    {
        public CheckService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(CheckForm form)
        {
            var tCheck = Table<Check>();

            var check = tCheck.GetOrAdd(w => w.ID == form.ID ||
                (form.ID == 0 && w.Type == form.Type && w.SourceID == form.SourceID && w.SourceTable == form.SourceTable));

            MapProperty(form, check, form.InlineEditProperty);
            UpdateAuditFields(check, form.ByUserID);
            // save
            SaveChanges();

            return check.ID;
        }
    }
}
