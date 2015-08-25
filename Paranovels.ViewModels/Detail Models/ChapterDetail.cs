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
        public List<ContentGrid> Contents { get; set; }
        public Novel Novel { get; set; }
        public List<Check> Checks { get; set; }
        public Summarize Summarize { get; set; }
        public List<Glossary> Glossaries { get; set; }
    }
}
