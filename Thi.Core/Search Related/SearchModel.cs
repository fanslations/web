using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public class SearchModel<T>
    {
        #region Properties

        /// <summary>
        /// Property - Gets or sets the Criteria
        /// </summary>
        /// <value>
        /// The Criteria
        /// </value>
        public T Criteria { get; set; }

        /// <summary>
        /// Property - Gets or sets the paged list config
        /// </summary>
        /// <value>
        /// The paged list config
        /// </value>
        public PagedListConfig PagedListConfig { get; set; }

        #endregion
    }
}
