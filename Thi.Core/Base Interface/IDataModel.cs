using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public interface IDataModel : IModel
    {
        int InsertedBy { get; set; }
        DateTime InsertedDate { get; set; }
        int UpdatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        bool IsDeleted { get; set; }
    }
}
