using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class BabelFishTranslator : ITranslator
    {
        class Session
        {
            public string lang_s { get; set; }
            public string lang_d { get; set; }
            public string phrase { get; set; }
            public string phrase_t { get; set; }
        }

        class BabelFish
        {
            public bool status { get; set; }
            public string message { get; set; }
            public bool cookie_id { get; set; }
            public Session session { get; set; }
        }

        public string Translate(string text, string from = "auto", string to = "auto")
        {
            try
            {
                // translate to special language code for BabelFish
                var language = new Dictionary<string, string>
                {
                    {"en", "en"},
                    {"ja", "ja"},
                    {"ko", "ko"},
                    {"zh", "zh-CHS"},
                };
                
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Encoding", "gzip, deflate");
                    webClient.Headers.Add("Accept-Language","en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    webClient.Headers.Add("Origin", "http://www.babelfish.com/");
                    webClient.Headers.Add("Referer", "http://www.babelfish.com/");

                    webClient.Headers.Add("Cookie", string.Format("skip_contest=1; lang_to=English; lang_from=Japanese; lang_to_code=en; lang_from_code=ja; PHPSESSID={0}", text.GetMd5Hash()));
                    var uri = new Uri("http://www.babelfish.com/tools/translate_files/ajax/session.php");
                    var requestDetails = string.Format("act=save_session&lang_s={1}&lang_d={2}&phrase={0}", HttpUtility.UrlEncode(text), language[from] ?? from, language[to] ?? to);

                    

                    var bytes = webClient.UploadData(uri, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    // have saved session, get translation from it
                    if (resultJson.Contains("phrase_t"))
                    {
                        var babelFish = JsonHelper.Deserialize<BabelFish>(resultJson);
                        return babelFish.session.phrase_t;
                    }

                    // do a second request to get the translate text

                    var uri2 = new Uri("http://www.babelfish.com/tools/translate_files/ajax/page5_success_page.php?userID=");
                    var bytes2 = webClient.DownloadData(uri2);
                    
                    var resultJson2 = Encoding.UTF8.GetString(bytes2);

                    var startIndex = resultJson2.LastIndexOf(@"<li class=""s_after"">", StringComparison.OrdinalIgnoreCase);
                    var endIndex = resultJson2.IndexOf("</li>", startIndex, StringComparison.OrdinalIgnoreCase);
                    if (startIndex > 0)
                    {
                        resultJson2 = resultJson2.Substring(startIndex, endIndex - startIndex);
                        return resultJson2.Replace(@"<li class=""s_after"">","");
                    }

                    return "";
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
                    ID = "BabelFish".GetIntHash(),
                    Name = "BabelFish",
                    Url = "http://www.babelfish.com/",
                    IsManual = false,
                };
            }
        }
    }
}
