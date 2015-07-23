using System;
using System.Collections.Specialized;

namespace Thi.Core
{
    /// <summary>
    /// This is config value for PagedList.
    /// </summary>
    [Serializable]
    public class PagedListConfig
    {
        public PagedListConfig ConfigWith(NameValueCollection nvc)
        {
            PageIndex = int.Parse(nvc[PageIndexKey] ?? "0");
            PageSize = int.Parse(nvc[PageSizeKey] ?? "0");
            OrderBy = nvc[OrderByKey];
            return this;
        }

        public string PageIndexKey
        {
            get { return Prefix + "pi"; }
        }

        public string PageSizeKey
        {
            get { return Prefix + "ps"; }
        }

        public string OrderByKey
        {
            get { return Prefix + "ob"; }
        }

        /// <summary>
        /// Gets or sets the total page.
        /// </summary>
        /// <value>
        /// The total page.
        /// </value>
        public int TotalPage { get; set; }
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }

        private int m_pageIndex;
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex
        {
            get { return this.m_pageIndex == 0 ? 1 : m_pageIndex; }
            set { this.m_pageIndex = value < 1 ? 1 : value; }
        }

        private int m_pageSize;
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize
        {
            get { return this.m_pageSize == 0 ? 20 : m_pageSize; }
            set { this.m_pageSize = value < 1 ? 20 : value; }
        }

        private string m_orderBy;
        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        /// <value>
        /// The order by.
        /// </value>
        public string OrderBy
        {
            get { return string.IsNullOrWhiteSpace(this.m_orderBy) ? null : this.m_orderBy; }
            set
            {
                this.m_orderBy = string.IsNullOrWhiteSpace(m_orderBy) ? value : m_orderBy;
            }
        }

       
        private string m_prefix;
        public string Prefix
        {
            get { return string.IsNullOrWhiteSpace(m_prefix) ? "" : m_prefix; }
            set { m_prefix = value; }
        }
    }
}
