using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thi.Core;

namespace Paranovels.ViewModels
{
    public class GenericCriteria<T> where T : class, IDataModel, new()
    {

        public T DataModel { get; set; }
    }
}
