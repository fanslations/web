using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class GroupForm : Group, IFormModel
    {
        public int ByUserID { get; set; }
        public string InlineEditProperty { get; set; }

        public IList<Feed> Feeds { get; set; }
        public IList<Glossary> Glossaries { get; set; }
        public IList<int> Categories { get; set; }
        public IList<int> Genres { get; set; }
        public IList<int> Contains { get; set; }
    }
}
