using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class CommentGrid: UserComment, IDetailModel
    {
        [NotMapped]
        public User User { get; set; }
        [NotMapped]
        public string Commenter
        {
            get
            {
                if (User == null) return "Unknown";
                if (!string.IsNullOrWhiteSpace(User.FirstName + User.LastName)) return User.FirstName + " " + User.LastName;
                if (!string.IsNullOrWhiteSpace(User.Username)) return User.Username;
                return "Guest" + User.ID;
            }
        }

        // from Summarize
        public int? CommentCount { get; set; }
        public int? ViewCount { get; set; }
        public int? VoteUp { get; set; }
        public int? VoteDown { get; set; }
        public int? QualityCount { get; set; }
        public int? QualityScore { get; set; }

        // by current user
        public int? Voted { get; set; }
        public int? QualityRated { get; set; }
        public bool? IsRead { get; set; }
    }
}
