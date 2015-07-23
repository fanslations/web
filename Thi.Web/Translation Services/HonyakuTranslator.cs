using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class HonyakuTranslator : ITranslator
    {
        class Result
        {
            public int key { get; set; }
            public string TranslateText { get; set; }
            public string TranslatedText { get; set; }
        }

        class ResultText
        {
            public IList<Result> Results { get; set; }
        }

        class Result2
        {
            public int key { get; set; }
            public string RequestText { get; set; }
            public string ResultText { get; set; }
            public string TranslateWords { get; set; }
            public string TranslatedWords { get; set; }
        }

        class ResultBilingual
        {
            public IList<Result2> Results { get; set; }
        }

        class ResultSet
        {
            public ResultText ResultText { get; set; }
            public ResultBilingual ResultBilingual { get; set; }
        }

        class Honyaku
        {
            public ResultSet ResultSet { get; set; }
        }

        public string Translate(string text, string from = "auto", string to = "auto")
        {
            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    webClient.Headers.Add("Origin", "http://honyaku.yahoo.co.jp/");
                    webClient.Headers.Add("Referer", "http://honyaku.yahoo.co.jp/");

                    var uri = new Uri("http://honyaku.yahoo.co.jp/TranslationText");
                    var requestDetails = string.Format("p={0}&ieid={1}&oeid={2}&results=1000&formality=0&_crumb={3}&output=json", HttpUtility.UrlEncode(text), from, to, GetCrumb());

                    var bytes = webClient.UploadData(uri, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    if (!string.IsNullOrWhiteSpace(resultJson) && resultJson.IndexOf("ResultSet", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        var honyaku = JsonHelper.Deserialize<Honyaku>(resultJson);
                        return string.Join(". ", honyaku.ResultSet.ResultText.Results.Select(s => s.TranslatedText));
                    }
                    return resultJson;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string GetCrumb()
        {
            using (var webClient = WebClientFactory.ChromeClient())
            {
                var html = webClient.DownloadString("http://honyaku.yahoo.co.jp/transtext");
                html = html.Substring(html.IndexOf(@"name=""TTcrumb"" value=""", StringComparison.Ordinal));
                html = html.Substring(0, html.IndexOf(@"""/>", StringComparison.Ordinal));

                return html.Replace(@"name=""TTcrumb"" value=""", "");
            }
        }

        public static TranslatorIdentity Identity
        {
            get
            {
                return new TranslatorIdentity
                {
                    ID = "Honyaku".GetIntHash(),
                    Name = "Honyaku",
                    Url = "http://honyaku.yahoo.co.jp/",
                    IsManual = false
                };
            }
        }
    }
}
