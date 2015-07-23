using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public interface IFormModel : IModel
    {
         int ByUserID { get; set; }
         string InlineEditProperty { get; set; }
    }
}
