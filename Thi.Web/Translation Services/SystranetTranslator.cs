using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class SystranetTranslator : ITranslator
    {
        public string Translate(string text, string from = "auto", string to = "auto")
        {
            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    webClient.Headers.Add("Origin", "http://www.systranet.com");
                    webClient.Headers.Add("Referer", "http://www.systranet.com/translate");

                    var url = string.Format("http://www.systranet.com/sai?lp={0}_{1}&service=systranettranslate", from, to);
                    var requestDetails = string.Format("[{0}]", text);

                    var bytes = webClient.UploadData(url, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    if (!string.IsNullOrWhiteSpace(resultJson) && resultJson.Length > text.Length)
                    {
                        resultJson = resultJson.Substring(resultJson.IndexOf("];[", StringComparison.Ordinal) + 3);
                        resultJson = resultJson.Substring(0, resultJson.Length - 2);
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
                    ID = "Systranet".GetIntHash(),
                    Name = "Systranet",
                    Url = "http://www.Systranet.com/",
                    IsManual = false
                };
            }
        }
    }
}
