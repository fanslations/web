using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class GlossaryCriteria : BaseCriteria
    {
        public IList<int> RawLanguages { get; set; }
    }
}
