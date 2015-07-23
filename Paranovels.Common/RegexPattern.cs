using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paranovels.Common
{
    public class RegexPattern
    {

        public const string ReleaseTitle =
            @"(\w+|\s|\-|\,)*?(\d+)(!)*";
    }
}
