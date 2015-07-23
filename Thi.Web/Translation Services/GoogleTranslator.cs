using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class GoogleTranslator : ITranslator
    {
        public string Translate(string text, string from = "auto", string to = "auto")
        {
            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    webClient.Headers.Add("Origin", "https://translate.google.com/");
                    webClient.Headers.Add("Referer", "https://translate.google.com/");


                    var url = string.Format("https://translate.google.com/translate_a/single?client=t&sl={1}&tl={2}&hl=en&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&dt=at&ie=UTF-8&oe=UTF-8&source=btn&srcrom=0&ssel=3&tsel=3&kc=0&tk=521427|965199&q={0}", HttpUtility.UrlEncode(text), from, to);
      
                    var bytes = webClient.DownloadData(url);
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    if (!string.IsNullOrWhiteSpace(resultJson) && resultJson.Length > text.Length)
                    {
                        resultJson = resultJson.Substring(resultJson.IndexOf(@"[[[""", StringComparison.Ordinal) + 4, resultJson.IndexOf(@""",""" + text.First(), StringComparison.Ordinal) - 4);
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
                    ID = "Google".GetIntHash(),
                    Name = "Google",
                    Url = "https://translate.google.com/",
                    IsManual = false
                };
            }
        }
    }
}
