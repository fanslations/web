using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class ReleaseGrid: Release, IDetailModel, IFeed
    {
        [NotMapped]
        public Group Group { get; set; }
        [NotMapped]
        public Series Series { get; set; }
        [NotMapped]
        public List<UserList> UserLists { get; set; }
        [NotMapped]
        public List<Connector> Connectors { get; set; }
        [NotMapped]
        public List<Tag> Tags { get; set; }

        [NotMapped]
        public string PrettyTitle
        {
            get
            {
                if (Series == null)
                {
                    return Title;
                }
                return Title.IsChapterRelease() ? Series.Title + " " + Title.ExtractVolumeChapter() : Title;
            }
        }

        // from Summarize
        public int? CommentCount { get; set; }
        public int? ViewCount { get; set; }
        public int? VoteUp { get; set; }
        public int? VoteDown { get; set; }
        public int? QualityCount { get; set; }
        public int? QualityScore { get; set; }

        // administrator controls
        public bool IsSticky { get; set; }

        // by current user
        public int? Voted { get; set; }
        public int? QualityRated { get; set; }
        public bool? IsRead { get; set; }

        // feed
        public string Content { get; set; }
    }
}
