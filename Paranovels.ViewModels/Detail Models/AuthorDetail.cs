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
    public class AuthorDetail: Author, IDetailModel
    {
        [NotMapped]
        public List<Series> Series { get; set; }
        [NotMapped]
        public List<Release> Releases { get; set; }
        [NotMapped]
        public Summarize Summarize { get; set; }
        [NotMapped]
        public UserActionDetail UserAction { get; set; }
        [NotMapped]
        public List<Aka> Akas { get; set; }
    }
}
