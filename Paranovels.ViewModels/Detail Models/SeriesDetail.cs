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
    public class SeriesDetail : Series, IDetailModel
    {
        [NotMapped]
        public List<UserList> UserLists { get; set; }
        [NotMapped]
        public List<Connector> Connectors { get; set; }
        [NotMapped]
        public List<Tag> Categories { get; set; }
        [NotMapped]
        public List<Tag> Genres { get; set; }
        [NotMapped]
        public List<Tag> Contains { get; set; }
        [NotMapped]
        public Group Group { get; set; }
        [NotMapped]
        public Summarize Summarize { get; set; }
        [NotMapped]
        public List<Release> Releases { get; set; }
        [NotMapped]
        public List<Feed> Feeds { get; set; }
        [NotMapped]
        public List<Glossary> Glossaries { get; set; }
        [NotMapped]
        public List<Aka> Akas { get; set; } // also known as (alternative title)
        [NotMapped]
        public UserActionDetail UserAction { get; set; }
    }
}
