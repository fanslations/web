using System;
using System.Collections.Generic;

namespace Thi.Core
{
    /// <summary>
    /// Wrapper class to hold pagedlist data and config.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PagedList<T>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IList<T> Data { get; set; }
        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public PagedListConfig Config { get; set; }

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public PagedListLabel Label { get; set; }

    }
}