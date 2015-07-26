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
    public class ReleaseGrid: Release, IDetailModel
    {
        [NotMapped]
        public Group Group { get; set; }
        [NotMapped]
        public Series Series { get; set; }

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
