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
    public class AkaService : BaseService
    {
        public AkaService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public int SaveChanges(AkaForm form)
        {
            var tAka = Table<Aka>();

            var aka = tAka.GetOrAdd(w => w.AkaID == form.AkaID);
            UpdateAuditFields(aka, form.ByUserID);
            MapProperty(form, aka, form.InlineEditProperty);
            // save
            SaveChanges();

            return aka.AkaID;
        }

        public AkaDetail Get(BaseCriteria criteria)
        {
            var qAka = View<Aka>().All();

            var aka = qAka.SingleOrDefault(w => w.AkaID == criteria.IDToInt);
            if (aka == null) return null;

            var detail = new AkaDetail();
            MapProperty(aka, detail);

            return detail;
        }
    }

}
