using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using SimpleFeedReader;
using Thi.Core;

namespace Paranovels.Proxies
{
    public class FeedChecker
    {
        public IEnumerable<ReleaseForm> Check(string url)
        {
            var reader = new FeedReader();
            var items = reader.RetrieveFeed(url);

            return items.Select(i => new ReleaseForm
            {
                Date = i.PublishDate.LocalDateTime,
                Title = i.Title, 
                Summary = Html2Text(i.Summary), 
                Url = i.Uri.ToString(),
                UrlHash = i.Uri.ToString().GetIntHash()
            });
        }

        string Html2Text(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";

            html = html.Replace("&nbsp;", " ")
                .Replace("</p>", "\r\n")
                .Replace("<br>", "\r\n")
                .Replace("<br/>", "\r\n")
                .Replace("<br />", "\r\n");
            html = Regex.Replace(html, @"<[^>]*>", String.Empty);
            return html.Trim();
        }
    }
}
