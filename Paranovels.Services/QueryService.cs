using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class QueryService : BaseService
    {
        public QueryService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }
    }
}
