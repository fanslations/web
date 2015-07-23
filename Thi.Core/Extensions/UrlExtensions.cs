using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Thi.Core
{
    public static class UrlExtension
    {
        public static string UrlRemovePageConfig(this string url, string prefix = "")
        {
            url = url.UrlRemoveQuery(prefix + "ob");
            url = url.UrlRemoveQuery(prefix + "pi");
            url = url.UrlRemoveQuery(prefix + "ps");
            return url;
        }
        /// <summary>
        /// Remove a key from the query string.
        /// </summary>
        /// <param name="url">String url.</param>
        /// <param name="queryName">Query string name.</param>
        /// <returns>
        /// String url with the request query string removed.
        /// </returns>
        public static string UrlRemoveQuery(this string url, string queryName)
        {
            // if there no '?' then add it
            if (!url.Contains("?")) { return url + "?"; }

            string path = url.Substring(0, url.IndexOf("?"));
            string query = url.Substring(url.IndexOf("?") + 1);
            NameValueCollection queryString = QueryStringToCollection(query);
            queryString.Remove(queryName);

            // rebuild query string
            query = String.Empty;
            for (int i = 0; i < queryString.Count; i++)
            {
                query += queryString.GetKey(i) + "=" + queryString.Get(i) + "&";
            }
            return path + "?" + query.TrimEnd('&');
        }

        /// <summary>
        /// Add query string to the url.
        /// </summary>
        /// <param name="url">Url string.</param>
        /// <param name="queryName">Name of query.</param>
        /// <param name="queryValue">Value of query.</param>
        /// <returns>Url string with new query string.</returns>
        public static string UrlAddQuery(this string url, string queryName, object queryValue)
        {
            url = url.UrlRemoveQuery(queryName);
            if (!url.EndsWith("?")) url += "&";
            return url + string.Format("{0}={1}", queryName, string.Format("{0}", queryValue).Trim()); // trim extra spacea
        }

        /// <summary>
        /// Convert query string to name value collection.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>NameValueCollection of the query string.</returns>
        public static NameValueCollection QueryStringToCollection(this string queryString)
        {
            var nvc = new NameValueCollection();

            // remove anything other than query string from url
            if (queryString.Contains("?"))
            {
                queryString = queryString.Substring(queryString.IndexOf('?') + 1);
            }

            foreach (string vp in Regex.Split(queryString, "&"))
            {
                string[] singlePair = Regex.Split(vp, "=");
                if (singlePair.Length == 2)
                {
                    nvc.Add(singlePair[0], singlePair[1]);
                }
                else
                {
                    // only one key with no value specified in query string
                    if (!String.IsNullOrEmpty(singlePair[0]))
                    {
                        nvc.Add(singlePair[0], String.Empty);
                    }
                }
            }

            return nvc;
        }

        public static string ToQueryString(this IDictionary<string, string> parameters)
        {
            if (parameters == null) return string.Empty;

            StringBuilder data = new StringBuilder();
            foreach (var key in parameters.Keys)
            {
                if (parameters[key] != null)
                    data.AppendFormat("{0}={1}&", key, parameters[key]);
            }
            return data.Remove(data.Length - 1, 1).ToString();
        }

        public static string UrlIsLinkUrl(this string url, string linkUrl, string returnValue)
        {
            if (url.Contains(linkUrl))
                return returnValue;
            return "";
        }

        public static string ToSeo(this string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "";
            title = Regex.Replace(title, @"([^a-zA-Z0-9])+", "-");
            return title;
        }
    }
}
