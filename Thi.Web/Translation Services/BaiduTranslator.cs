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
    public class BaiduTranslator : ITranslator
    {
        class Data
        {
            public string dst { get; set; }
            public string src { get; set; }
            public List<object> relation { get; set; }
            public List<List<object>> result { get; set; }
        }

        class Baidu
        {
            public string from { get; set; }
            public string to { get; set; }
            public string domain { get; set; }
            public int type { get; set; }
            public int status { get; set; }
            public List<Data> data { get; set; }
        }

        public string Translate(string text, string from = "auto", string to = "auto")
        {
            try
            {
                
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Encoding", "gzip, deflate");
                    webClient.Headers.Add("Accept-Language","en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    webClient.Headers.Add("Origin", "http://translate.baidu.com");
                    webClient.Headers.Add("Referer", "http://translate.baidu.com");

                    var uri = new Uri("http://translate.baidu.com/transcontent");
                    var requestDetails = string.Format("ie=utf-8&source=txt&query={0}&from={1}&to={2}&token=4b8ef49c152664896591cb3c27610ce6", HttpUtility.UrlEncode(text), from, to);
                    var bytes = webClient.UploadData(uri, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    var result = JsonHelper.Deserialize<Baidu>(resultJson);
                    return string.Join(" ", result.data.Select(s=>s.dst));
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
                    ID = "Baidu".GetIntHash(),
                    Name = "Baidu",
                    Url = "http://translate.baidu.com",
                    IsManual = false
                };
            }
        }
    }
}
