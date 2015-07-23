using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class VoteCriteria : BaseCriteria
    {
        public int SourceID { get; set; }
        public int SourceTable { get; set; }
        public int Vote { get; set; }
    }
}
