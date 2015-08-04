using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class UserDetail : User, IDetailModel
    {
        [NotMapped]
        public IList<UserPreference> Preferences { get; set; }
        [NotMapped]
        public IList<UserList> Lists { get; set; }

        public IList<int> HideSeriesIDs { get; set; } 
    }
}
