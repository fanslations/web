using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class LecTranslator : ITranslator
    {
        public string Translate(string text, string from = "auto", string to = "auto")
        {
            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    webClient.Headers.Add("Origin", "http://www.lec.com/translate-demos.asp");
                    webClient.Headers.Add("Referer", "http://www.lec.com/translate-demos.asp");

                    var uri = new Uri("http://www.lec.com/translate-demos.asp");
                    var requestDetails = string.Format("selectSourceLang={1}&selectTargetLang={2}&DoTransText=go&SourceText={0}", HttpUtility.UrlEncode(text), from, to);

                    var bytes = webClient.UploadData(uri, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    if (!string.IsNullOrWhiteSpace(resultJson) && resultJson.IndexOf("<textarea readonly", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        resultJson = resultJson.Substring(resultJson.IndexOf("<textarea readonly", StringComparison.OrdinalIgnoreCase));
                        resultJson = resultJson.Substring(resultJson.IndexOf(">", StringComparison.OrdinalIgnoreCase) + 1);
                        resultJson = resultJson.Substring(0, resultJson.IndexOf("</textarea>", StringComparison.OrdinalIgnoreCase)).Trim();
                    }
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
                    ID = "LEC".GetIntHash(),
                    Name = "LEC",
                    Url = "http://www.lec.com/",
                    IsManual = false
                };
            }
        }
    }
}
