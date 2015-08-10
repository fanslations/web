using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    [Serializable]
    public class PagedListLabel
    {
        public string PageMOfN { get; set; }
        public string First { get; set; }
        public string Previous { get; set; }
        public string Next { get; set; }
        public string Last { get; set; }
        public string PerPage { get; set; }
    }
}
