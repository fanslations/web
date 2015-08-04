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
    public class PreferenceService : BaseService
    {
        public PreferenceService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public int SaveChanges(PreferenceForm form)
        {
            var tUserPreference = Table<UserPreference>();

            var userPreference = tUserPreference.GetOrAdd(w => w.UserPreferenceID == form.UserPreferenceID ||
                (w.UserID == form.UserID && w.Type == form.Type && w.SourceID == form.SourceID && w.SourceTable == form.SourceTable));

            MapProperty(form, userPreference, form.InlineEditProperty);
            UpdateAuditFields(userPreference, form.ByUserID);

            // override default for score since it can be 0
            userPreference.Score = form.Score;
     

            // save
            SaveChanges();

            return userPreference.UserPreferenceID;
        }
    }
}
