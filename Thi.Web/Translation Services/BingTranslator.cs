using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class BingTranslator : ITranslator
    {
        protected class AccessToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string scope { get; set; }
        }

        public string Translate(string text, string from, string to)
        {
            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    var token = GetAccessToken();

                    string uri = string.Format("http://api.microsofttranslator.com/v2/Http.svc/Translate?text={0}&from={1}&to={2}", HttpUtility.UrlEncode(text), from, to);
                    webClient.Headers.Add("Authorization", string.Format("Bearer {0}", token.access_token));
                    var bytes = webClient.DownloadData(uri);

                    using (Stream stream = new MemoryStream(bytes))
                    {
                        var dcs = new DataContractSerializer(typeof(string));
                        return (string) dcs.ReadObject(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }

        AccessToken GetAccessToken()
        {
            var clientID = HttpUtility.UrlEncode("paranovels");
            var clientSecret = HttpUtility.UrlEncode("Qyyv9xNOQmBfYijLaHzMwkhvdnYSX6ppJ385txXoWV0=");

            const string translatorAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
            var requestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",clientID, clientSecret);


            using (var webClient = WebClientFactory.ChromeClient())
            {
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                string resultJson = webClient.UploadString(translatorAccessUri, requestDetails);

                return JsonHelper.Deserialize<AccessToken>(resultJson);
            }
        }

        public static TranslatorIdentity Identity
        {
            get
            {
                return new TranslatorIdentity
                {
                    ID = "Bing".GetIntHash(),
                    Name = "Bing",
                    Url = "http://www.bing.com/",
                    IsManual = false
                };
            }
        }
    }
}
