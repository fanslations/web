using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thi.Core;

namespace Thi.Web
{
    public class YoudaoTranslator : ITranslator
    {
        class SmartResult
        {
            public int type { get; set; }
            public List<string> entries { get; set; }
        }
        class Translated
        {
            public string src { get; set; }
            public string tgt { get; set; }
        }
        class Youdao
        {
            public string type { get; set; }
            public int errorCode { get; set; }
            public int elapsedTime { get; set; }
            public List<List<Translated>> translateResult { get; set; }
            public SmartResult smartResult { get; set; }
        }

        public string Translate(string text, string from = "auto", string to = "auto")
        {
            try
            {
                using (var webClient = WebClientFactory.ChromeClient())
                {
                    webClient.Headers.Add("Accept-Language", "en-US,en;q=0.8,vi;q=0.6");
                    webClient.Headers.Add("Accep", "application/json, text/javascript, */*; q=0.01");
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    webClient.Headers.Add("Origin", "http://fanyi.youdao.com/");
                    webClient.Headers.Add("Referer", "http://fanyi.youdao.com/");

                    var urldecode = HttpUtility.UrlDecode("%C3%A6%C2%83%C2%B3%C3%A8%C2%A6%C2%81%C3%A6%C2%88%C2%90%C3%A4%C2%B8%C2%BA");
                    var uri = new Uri("http://fanyi.youdao.com/translate");
                    var requestDetails = string.Format("type={1}2{2}&i={0}&doctype=json&xmlVersion=1.8&keyfrom=fanyi.web&ue=UTF-8&action=FY_BY_CLICKBUTTON&typoResult=true", (text), from, to);
                    //requestDetails = "type=zh2en&i=%C3%A6%C2%83%C2%B3%C3%A8%C2%A6%C2%81%C3%A6%C2%88%C2%90%C3%A4%C2%B8%C2%BA&doctype=json&xmlVersion=1.8&keyfrom=fanyi.web&ue=UTF-8&action=FY_BY_CLICKBUTTON&typoResult=true";
                    var bytes = webClient.UploadData(uri, Encoding.UTF8.GetBytes(requestDetails));
                    var resultJson = Encoding.UTF8.GetString(bytes);

                    if (!string.IsNullOrWhiteSpace(resultJson) && resultJson.IndexOf("translateResult", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        var youdao = JsonHelper.Deserialize<Youdao>(resultJson);
                        return string.Join(". ", youdao.translateResult.Select(s => string.Join(". ", s.Select(s1 => s1.tgt))));
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
                    ID = "Youdao".GetIntHash(),
                    Name = "Youdao",
                    Url = "http://fanyi.youdao.com",
                    IsManual = true
                };
            }
        }
    }
}
