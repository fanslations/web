using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thi.Core;

namespace Paranovels.Facade
{
    public class FacadeFactory
    {
        private static readonly List<object> CacheStore = new List<object>(); 
        public static T Create<T>()  where T : class, IFacade
        {
            if (CacheStore.Exists(w => w.GetType() == typeof(T)))
            {
                return CacheStore.First(w => w.GetType() == typeof (T)) as T;
            }
            var facade = Activator.CreateInstance(typeof(T));
            if (!CacheStore.Contains(facade))
            {
                CacheStore.Add(facade);
            }
            return facade as T;
        }
    }
}
