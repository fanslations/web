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
        public List<Tag> Genres { get; set; }
        public List<Tag> Categories { get; set; }
        public List<Group> Groups { get; set; }
        public List<Aka> Akas { get; set; }
        public string RawUrl { get; set; }
        public Summarize Summarize { get; set; }
        public List<Glossary> Glossaries { get; set; }
    }
}
