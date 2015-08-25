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
    public class ContentGrid : Content, IDetailModel
    {
        public int Paragraph { get; set; }
        public string Raw { get; set; }
        public bool IsTranslated { get; set; }
        public bool IsEdited { get; set; }
        public bool IsProofread { get; set; }

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
