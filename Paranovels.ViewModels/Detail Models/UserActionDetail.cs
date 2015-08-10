using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paranovels.ViewModels
{
    public class UserActionDetail
    {
        public int? Voted { get; set; }
        public int? QualityRated { get; set; }
        public bool? IsRead { get; set; }
    }
}
