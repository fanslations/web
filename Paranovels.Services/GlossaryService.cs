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
    public class GlossaryService : BaseService
    {
        public GlossaryService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(GlossaryForm form)
        {
            var tGlossary = Table<Glossary>();

            var glossary = tGlossary.GetOrAdd(w => w.ID == form.ID);
            MapProperty(form, glossary, form.InlineEditProperty);
            UpdateAuditFields(glossary, form.ByUserID);
            // save
            SaveChanges();

            return glossary.ID;
        }
    }
}
