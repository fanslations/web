using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class FreeTranslationTranslator : ITranslator
    {
        class FreeTranslation
        {
            public string translation { get; set; }
        }

        public string Translate(string text, string from = "auto", string to = "auto")
        {
            var language = new Dictionary<string, string>
            {
                {"en", "eng"},
                {"ja", "jpn"},
                {"ko", "kor"},
                {"zh", "chi"},
                {"fr", "fra"},
                {"it", "ita"},
                {"es", "spa"},
                {"de", "ger"},
                {"pt", "por"},
                {"vi","vie"}
            };
        
            if (language.ContainsKey(from) == false || language.ContainsKey(to) == false)
                return string.Format("This translator does not support translating from {0} to {1}", from, to);

            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/json");
                    webClient.Headers.Add("Tracking", "applicationKey=dlWbNAC2iLJWujbcIHiNMQ%3D%3D applicationInstance=freetranslation");
                    webClient.Headers.Add("Origin", "http://www.freetranslation.com/");
                    webClient.Headers.Add("Referer", "http://www.freetranslation.com/");

                    var uri = new Uri("http://www.freetranslation.com/gw-mt-proxy-service-web/mt-translation");
                    var requestDetails = string.Format(@"{{""text"":""{0}"",""from"":""{1}"",""to"":""{2}""}}", HttpUtility.UrlEncode(text), language[from], language[to]);

                    var bytes = webClient.UploadData(uri, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    if (!string.IsNullOrWhiteSpace(resultJson) && resultJson.IndexOf("translation", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        var freeTranslation = JsonHelper.Deserialize<FreeTranslation>(resultJson);
                        return freeTranslation.translation;
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
                    ID = "Freetranslation".GetIntHash(),
                    Name = "Freetranslation",
                    Url = "http://www.freetranslation.com/",
                    IsManual = false
                };
            }
        }
    }
}
