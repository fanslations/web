using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class ConnectorCriteria : BaseCriteria
    {
        public int ConnectorType { get; set; }
        public int SourceID { get; set; }
        public int TargetID { get; set; }
    }
}
