using System;
using System.Linq;

namespace Thi.Core
{
    /// <summary>
    /// Extention methods class.
    /// </summary>
    public static class PagedListExtensions
    {
        /// <summary>
        /// Convert IQueryable to paged list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The IQueryable source.</param>
        /// <param name="pagedListConfig">The paged list config.</param>
        /// <returns>
        ///   <c>PagedList</c>
        /// </returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, PagedListConfig pagedListConfig)
        {
            var pagedList = new PagedListData<T>(source, pagedListConfig);
            var plw = new PagedList<T> { Data = pagedList, Config = pagedListConfig };

            return plw;
        }

        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }

            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        public static string AlphaNavigation<T>(this PagedList<T> source, string reqUrl, string fieldName, string fieldValue)
        {
            const string alphaNavigation = "All,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

            const string li = @"<li class=""{0}""><a href=""{1}"" title=""{2}"">{2}</a></li>";
            string html = "";

            foreach (string alpha in alphaNavigation.Split(','))
            {
                html += string.Format(li, fieldValue == alpha ? "active" : "", reqUrl.UrlAddQuery(fieldName, alpha), alpha);
            }
            return "<ul>" + html + "</ul>";
        }
        /// <summary>
        /// Build pagination for the specified page info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageInfo">The page info.</param>
        /// <param name="reqUrl">The req URL.</param>
        /// <param name="showPage">The show page.</param>
        /// <returns></returns>
        public static string ToPagination<T>(this PagedList<T> pageInfo, string reqUrl, int showPage = 5)
        {
            reqUrl = reqUrl.UrlRemoveQuery(pageInfo.Config.PageIndexKey);  // reqUrl.UrlRemoveQuery("pageindex");
            reqUrl = reqUrl.UrlRemoveQuery(pageInfo.Config.PageSizeKey);  // reqUrl.UrlRemoveQuery("pagesize");
            reqUrl = reqUrl.UrlRemoveQuery(pageInfo.Config.OrderByKey);  // reqUrl.UrlRemoveQuery("orderby");
            reqUrl += reqUrl.Contains("?") ? (reqUrl.EndsWith("&amp;") || reqUrl.EndsWith("?") ? string.Empty : "&amp;") : "?";
            reqUrl += string.Format("{0}={1}&amp;{2}={3}&amp;{4}={5}",
                pageInfo.Config.PageIndexKey, "{0}", pageInfo.Config.PageSizeKey, pageInfo.Config.PageSize, pageInfo.Config.OrderByKey, pageInfo.Config.OrderBy);

            reqUrl = reqUrl.Replace("&amp;", "&");

            int currentPage = pageInfo.Config.PageIndex;
            string pagination = String.Empty;
            int prevPage = pageInfo.Config.PageIndex - 1;
            int nextPage = pageInfo.Config.PageIndex + 1;
            var lastPage = (int)Math.Ceiling(pageInfo.Config.TotalCount / (decimal)pageInfo.Config.PageSize);
            int adjacents = showPage / 2;

            var paginationLabel = new
            {
                PageMOfN = "Page {0} of {1}",
                First = "«First",
                Previous = "‹Prev",
                Next = "Next›",
                Last = "Last»",
                PerPage = "Shows {0}"
            };
            const string strCurrentPage = @"<li class=""active""><a>{0}</a></li>";
            string strOtherPage = string.Format(@"<li><a href=""{0}"">{{0}}</a></li>", reqUrl.Replace("&", "&amp;"));
            string strFirstPage = string.Format(@"<li class=""{{1}}""><a href=""{0}"">{1}</a></li>", reqUrl.Replace("&", "&amp;"), paginationLabel.First);
            string strPrevPage = string.Format(@"<li class=""{{1}}""><a href=""{0}"">{1}</a></li>", reqUrl.Replace("&", "&amp;"), paginationLabel.Previous);
            string strNextPage = string.Format(@"<li class=""{{1}}""><a href=""{0}"">{1}</a></li>", reqUrl.Replace("&", "&amp;"), paginationLabel.Next);
            string strLastPage = string.Format(@"<li class=""{{1}}""><a href=""{0}"">{1}</a></li>", reqUrl.Replace("&", "&amp;"), paginationLabel.Last);
            const string strDisablePage = @"<li class=""disabled""><a>{0}</a></li>";


            int counter = 0;
            if (lastPage > 1)
            {
                //pagination += "<div class=\"pagination\">";
                // page m of n
                var jumpUrl = reqUrl.UrlRemoveQuery(pageInfo.Config.PageIndexKey);
                string jumpHtml = string.Format(@"
<div class=""text-center dropdown-container"">
    <form method=""get"" action=""{0}"" data-get>
        <label><span class=""sr-only"">Jump to Page</span><input type=""number"" name=""{1}"" class=""form-control input-sm"" placeholder=""Page"" required /></label>
        <button type=""submit"" class=""btn btn-sm btn-primary width-auto""><i class=""fa fa-rocket""></i>Go</button>
    </form>
</div>", jumpUrl.Replace("&", "&amp;"), pageInfo.Config.PageIndexKey);
                //jumpHtml = "";
                pagination += string.Format(@"<li class=""active""><a href=""#jumptopage""  class=""dropdown-switcher"" onclick=""$(this).parent().toggleClass('dropdown-open')"" title=""Jump to another page"">{0}<i class=""fa fa-sort-desc""></i></a>{1}</li>", string.Format(paginationLabel.PageMOfN, currentPage, lastPage), jumpHtml);
                if (adjacents != 0)
                {
                    // first button
                    pagination += string.Format(strFirstPage, 1, currentPage > 1 ? "active first" : "disabled first");
                }
                // previous button
                pagination += string.Format(strPrevPage, prevPage, currentPage > 1 ? "active previous" : "disabled previous");

                // pages	
                if (lastPage < 7 + (adjacents * 2))
                {
                    if (adjacents != 0)
                    {
                        // not enough pages to bother breaking it up
                        for (counter = 1; counter <= lastPage; counter++)
                        {
                            if (counter == currentPage)
                            {
                                pagination += string.Format(strCurrentPage, counter);
                            }
                            else
                            {
                                pagination += string.Format(strOtherPage, counter);
                            }
                        }
                    }
                }
                else if (lastPage > 5 + (adjacents * 2))
                {
                    if (adjacents != 0)
                    {
                        // enough pages to hide some
                        // close to beginning; only hide later pages
                        if (currentPage < 1 + (adjacents * 2))
                        {
                            if (adjacents != 0)
                            {
                                for (counter = 1; counter < 4 + (adjacents * 2); counter++)
                                {
                                    if (counter == currentPage)
                                    {
                                        pagination += string.Format(strCurrentPage, counter);
                                    }
                                    else
                                    {
                                        pagination += string.Format(strOtherPage, counter);
                                    }
                                }
                                pagination += @"<li><span class=""no-style"">...</span></li>";
                                pagination += string.Format(strOtherPage, lastPage - 1);
                                pagination += string.Format(strOtherPage, lastPage);
                            }
                        }
                    }
                    else if (lastPage - (adjacents * 2) > currentPage && currentPage > (adjacents * 2))
                    {
                        if (adjacents != 0)
                        {
                            // in middle; hide some front and some back
                            pagination += string.Format(strOtherPage, 1);
                            pagination += string.Format(strOtherPage, 2);
                            pagination += @"<li><span class=""no-style"">...</span></li>";
                            for (counter = currentPage - adjacents; counter <= currentPage + adjacents; counter++)
                            {
                                if (counter == currentPage)
                                {
                                    pagination += string.Format(strCurrentPage, counter);
                                }
                                else
                                {
                                    pagination += string.Format(strOtherPage, counter);
                                }
                            }
                            pagination += @"<li><span class=""no-style"">...</span></li>";
                            pagination += string.Format(strOtherPage, lastPage - 1);
                            pagination += string.Format(strOtherPage, lastPage);
                        }
                    }
                    else
                    {
                        if (adjacents != 0)
                        {
                            // close to end; only hide early pages
                            pagination += string.Format(strOtherPage, 1);
                            pagination += string.Format(strOtherPage, 2);
                            pagination += @"<li><span class=""no-style"">...</span></li>";
                            for (counter = lastPage - (2 + (adjacents * 2)); counter <= lastPage; counter++)
                            {
                                if (counter == currentPage)
                                {
                                    pagination += string.Format(strCurrentPage, counter);
                                }
                                else
                                {
                                    pagination += string.Format(strOtherPage, counter);
                                }
                            }
                        }
                    }
                }

                // next button
                pagination += string.Format(strNextPage, nextPage, currentPage < lastPage ? "active next" : "disabled next");
                if (adjacents != 0)
                {
                    // last button
                    pagination += string.Format(strLastPage, lastPage, currentPage < lastPage ? "active last" : "disabled last");

                }
                // per page
                var perPageUrl = reqUrl.UrlRemoveQuery(pageInfo.Config.PageIndexKey);
                perPageUrl = perPageUrl.UrlRemoveQuery(pageInfo.Config.PageSizeKey);
                string perPageHtml = string.Format(@"
<div class=""text-center dropdown-container"">
    <form method=""get"" action=""{0}"" data-get>
        <label><span class=""sr-only"">Show per Page</span><input type=""number"" name=""{1}"" class=""form-control input-sm"" placeholder=""Per Page"" required /></label>
        <button type=""submit"" class=""btn btn-sm btn-primary width-auto""><i class=""fa fa-bars""></i>Go</button>
    </form>
</div>", perPageUrl.Replace("&", "&amp;"), pageInfo.Config.PageSizeKey);
                //perPageHtml = "";
                pagination += string.Format(@"<li class=""active""><a href=""#perpage"" class=""dropdown-switcher"" onclick=""$(this).parent().toggleClass('dropdown-open')"" title=""Show more per page"">{0}<i class=""fa fa-sort-desc""></i></a>{1}</li>", string.Format(paginationLabel.PerPage, pageInfo.Config.PageSize), perPageHtml);

                return string.Format(@"<div class=""pagination-container""><ul class=""pagination"">{0}</ul></div>", pagination);
            }
            return null;
        }
    }
}
