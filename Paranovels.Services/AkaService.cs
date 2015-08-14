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

            var aka = tAka.GetOrAdd(w => w.ID == form.ID);
            UpdateAuditFields(aka, form.ByUserID);
            MapProperty(form, aka, form.InlineEditProperty);
            // save
            SaveChanges();

            return aka.ID;
        }

        public AkaDetail Get(BaseCriteria criteria)
        {
            var qAka = View<Aka>().All();

            var aka = qAka.SingleOrDefault(w => w.ID == criteria.IDToInt);
            if (aka == null) return null;

            var detail = new AkaDetail();
            MapProperty(aka, detail);

            return detail;
        }

        public IQueryable<Series> Union(IQueryable<Series> qSeries)
        {
            throw new NotImplementedException();
        }
    }

}
