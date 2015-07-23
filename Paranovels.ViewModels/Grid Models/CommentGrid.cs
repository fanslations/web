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
                return "Anonymous User" + User.UserID;
            }
        }
    }
}
