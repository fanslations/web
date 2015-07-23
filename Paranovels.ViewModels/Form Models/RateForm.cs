using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class RateForm : UserRate, IFormModel
    {
        public int ByUserID { get; set; }
        public string InlineEditProperty { get; set; }
    }
}
