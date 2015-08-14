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
    public class SpamReportService : BaseService
    {
        public SpamReportService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public int SaveChanges(SpamReportForm form)
        {
            var tSpamReport = Table<SpamReport>();

            var spamReport = tSpamReport.GetOrAdd(w => w.ID == form.ID);
            UpdateAuditFields(spamReport, form.ByUserID);
            MapProperty(form, spamReport, form.InlineEditProperty);
            // save
            SaveChanges();

            return spamReport.ID;
        }
    }
}
