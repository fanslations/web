using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class GroupDetail: Group, IDetailModel
    {
        public List<Release> Releases { get; set; }
        public List<Feed> Feeds { get; set; }
        public List<Glossary> Glossaries { get; set; }
    }
}
