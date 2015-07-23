using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public interface ICriteriaModel
    {
        string Query { get; set; }
        string SearchType { get; set; }

        Dictionary<string, string> HiddenFields { get; set; }
    }
}
