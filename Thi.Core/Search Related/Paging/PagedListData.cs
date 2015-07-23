using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Thi.Core
{
    /// <summary>
    /// This is a extention of the List class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PagedListData<T> : List<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedListData&lt;T&gt;"/> class.
        /// </summary>
        public PagedListData() {}
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedListData&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="pagedListConfig">The paged list config.</param>
        public PagedListData(IQueryable<T> source, PagedListConfig pagedListConfig)
        {
            TotalCount = source.Count();

            PageIndex = pagedListConfig.PageIndex;
            PageSize = pagedListConfig.PageSize;
            TotalPage = (int)Math.Ceiling(TotalCount / (decimal)PageSize);

            // if source is not already ordered then get order by
            if (!source.IsOrdered())
            {
                OrderBy = pagedListConfig.OrderBy ?? source.ElementType.GetProperties().First(w => !w.GetCustomAttributes(typeof(NotMappedAttribute), true).Any()).Name; // default to first field if no orderby was supplied
                source = source.OrderBy(OrderBy);
            }

            var queryResult = source.Skip<T>((PageIndex - 1) * PageSize).Take<T>(PageSize).ToList<T>();
            AddRange(queryResult);

            
            // copy back to pagedListConfig
            pagedListConfig.TotalPage = TotalPage;
            pagedListConfig.TotalCount = TotalCount;
            pagedListConfig.OrderBy = OrderBy;

        }

        #region IPagedList Members

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the total page.
        /// </summary>
        /// <value>
        /// The total page.
        /// </value>
        public int TotalPage { get; set; }
        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        /// <value>
        /// The order by.
        /// </value>
        public string OrderBy { get; set; }

       #endregion
    }
}