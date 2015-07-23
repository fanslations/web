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
    public class ExciteTranslator : ITranslator
    {
        public string Translate(string text, string from = "auto", string to = "auto")
        {
            try
            {
                var language = new Dictionary<string, string>
                {
                    {"en", "EN"},
                    {"ja", "JA"},

                };

                if (language.ContainsKey(from) == false || language.ContainsKey(to) == false)
                    return string.Format("This translator does not support translating from {0} to {1}", from, to);

                using (var webClient = WebClientFactory.ChromeClient())
                {
                    //webClient.Headers.Add("Accept-Encoding", "gzip, deflate");
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    webClient.Headers.Add("Origin", "http://www.excite.co.jp/world/english_japanese/");
                    webClient.Headers.Add("Referer", "http://www.excite.co.jp/world/english_japanese/");
                    webClient.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
                    //webClient.Headers.Add("Cookie", "xtl_s=723cf7ad55a5713f47403; registered=no; UID=6EA8403F55A57119");

                    var uri = new Uri("http://www.excite.co.jp/world/english_japanese/");
                    var requestDetails = string.Format("wb_lp={1}{2}&before={0}", HttpUtility.UrlEncode(text), from, to);



                    var bytes = webClient.UploadData(uri, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    if (resultJson.Contains(@"name=""after"">"))
                    {
                        var startIndex = resultJson.IndexOf(@"name=""after"">", StringComparison.OrdinalIgnoreCase);
                        var endIndex = resultJson.IndexOf(@"</textarea>", startIndex, StringComparison.OrdinalIgnoreCase);
                        resultJson = resultJson.Substring(startIndex, endIndex - startIndex);
                        return resultJson.Replace(@"name=""after"">", "");
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
                    ID = "Excite".GetIntHash(),
                    Name = "Excite",
                    Url = "http://www.excite.co.jp/",
                    IsManual = false
                };
            }
        }
    }
}
