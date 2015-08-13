using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using SimpleFeedReader;
using Thi.Core;
using Thi.Web;

namespace Paranovels.Proxies
{
    public class FeedChecker
    {
        public IEnumerable<ReleaseForm> Check(string url)
        {
            //ServicePointManager.Expect100Continue = false;
            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            //ServicePointManager.CheckCertificateRevocationList = false;

            var reader = new FeedReader();

            var items = reader.RetrieveFeed(url);

            return items.Any()
                ? items.Select(i => new ReleaseForm
                {
                    Date = i.PublishDate.LocalDateTime,
                    Title = i.Title,
                    Summary = Html2Text(i.Summary),
                    Url = i.Uri.ToString(),
                    UrlHash = i.Uri.ToString().GetIntHash()
                })
                : CheckUsingWebClient(url);
        }

        public IEnumerable<ReleaseForm> CheckUsingWebClient(string url)
        {
            using (var webClient = WebClientFactory.ChromeClient())
            {
                try
                {
                    var xmlString = webClient.DownloadString(url);
                    var reader = new FeedReader();
                    var items = reader.RetrieveFeed(XmlReader.Create(new StringReader(xmlString)));

                    return items.Select(i => new ReleaseForm
                    {
                        Date = i.PublishDate.LocalDateTime,
                        Title = i.Title,
                        Summary = Html2Text(i.Summary),
                        Url = i.Uri.ToString(),
                        UrlHash = i.Uri.ToString().GetIntHash()
                    });
                }
                catch (Exception ex) {}
            }
            return new List<ReleaseForm>();
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
