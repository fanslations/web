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
    public class ReleaseDetail: Release, IDetailModel
    {
        [NotMapped]
        public List<UserList> UserLists { get; set; }
        [NotMapped]
        public List<Connector> Connectors { get; set; }
        [NotMapped]
        public Series Series { get; set; }
        [NotMapped]
        public Group Group { get; set; }
        [NotMapped]
        public Summarize Summarize { get; set; }
        [NotMapped]
        public Sticky Sticky { get; set; }
        // by current user
        [NotMapped]
        public UserActionDetail UserAction { get; set; }

    }
}
