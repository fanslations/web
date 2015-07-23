using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class InfoSeekTranslator : ITranslator
    {
        class T
        {
            public string text { get; set; }
        }

        class InfoSeek
        {
            public IList<T> t { get; set; }
        }

        public string Translate(string text, string from = "auto", string to = "auto")
        {
            var language = new Dictionary<string, string>
            {
                {"en", "E"},
                {"ja", "J"},
                {"ko", "K"},
                {"zh", "C"},
                {"fr", "F"},
                {"it", "I"},
                {"es", "S"},
                {"de", "G"},
                {"pt", "P"}
            };

            if (language.ContainsKey(from) == false || language.ContainsKey(to) == false)
                return string.Format("This translator does not support translating from {0} to {1}", from, to);

            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    webClient.Headers.Add("Cross-Licence", "infoseek/main e3f33620ae053e48cdba30a16b1084b5d69a3a6c");
                    webClient.Headers.Add("Origin", "http://translation.infoseek.ne.jp/");
                    webClient.Headers.Add("Referer", "http://translation.infoseek.ne.jp/");

                    var uri = new Uri("http://translation.infoseek.ne.jp/clsoap/translate");
                    var requestDetails = string.Format("e={1}{2}&t={0}", HttpUtility.UrlEncode(text), language[from] ?? "J", language[to] ?? "E");

                    var bytes = webClient.UploadData(uri, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    var infoSeek = JsonHelper.Deserialize<InfoSeek>(resultJson);

                    if (infoSeek.t != null)
                        return infoSeek.t[0].text;

                    return resultJson;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static TranslatorIdentity Identity
        {
            get
            {
                return new TranslatorIdentity
                {
                    ID = "InfoSeek".GetIntHash(),
                    Name = "InfoSeek",
                    Url = "http://translation.infoseek.ne.jp/",
                    IsManual = false
                };
            }
        }
    }
}
