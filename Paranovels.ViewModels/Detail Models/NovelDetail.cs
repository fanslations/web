using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class NovelDetail: Novel, IDetailModel
    {
        public List<Chapter> Chapters { get; set; }
        public List<Author> Authors { get; set; }
    }
}
