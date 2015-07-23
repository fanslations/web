using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class ChapterDetail: Chapter, IDetailModel
    {
        public List<Content> Contents { get; set; }
    }
}
