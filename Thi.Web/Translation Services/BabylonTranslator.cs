using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class BabylonTranslator : ITranslator
    {
        class Babylon
        {
            public string translatedText { get; set; }
        }

        public string Translate(string text, string from = "auto", string to = "auto")
        {
            var language = new Dictionary<string, string>
            {
                {"en", "0"},
                {"ja", "8"},
                {"ko", "12"},
                {"zh", "10"},
                {"fr", "1"},
                {"it", "2"},
                {"es", "3"},
                {"de", "6"},
                {"pt", "5"}
            };

            if (language.ContainsKey(from) == false || language.ContainsKey(to) == false)
                return string.Format("This translator does not support translating from {0} to {1}", from, to);

            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    webClient.Headers.Add("Origin", "http://translation.babylon.com/");
                    webClient.Headers.Add("Referer", "http://translation.babylon.com/");


                    var url = string.Format("http://translation.babylon.com/translate/babylon.php?v=1.0&langpair={1}%7C{2}&callback=ret&q={0}", HttpUtility.UrlEncode(text), language[from], language[to]);
      
                    var bytes = webClient.DownloadData(url);
                    var resultJson = Encoding.UTF8.GetString(bytes); // ret('', {"translatedText":"Feed."}, 200, null, null);

                    if (!string.IsNullOrWhiteSpace(resultJson) && resultJson.Contains(@"{""translatedText"":"))
                    {
                        resultJson =
                            resultJson.Substring(resultJson.IndexOf(@"{""translatedText"":", StringComparison.Ordinal));
                        resultJson = resultJson.Substring(0, resultJson.IndexOf(@"},", StringComparison.Ordinal) + 1);

                        var babylon = JsonHelper.Deserialize<Babylon>(resultJson);

                        return babylon.translatedText;
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
                    ID = "Babylon".GetIntHash(),
                    Name = "Babylon",
                    Url = "http://translation.babylon.com/",
                    IsManual = false
                };
            }
        }

    }

}
