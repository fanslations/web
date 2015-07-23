using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;

namespace Thi.Web
{
    public class DetectLanguageDetector
    {
        class Detection
        {
            public string language { get; set; }
            public bool isReliable { get; set; }
            public float confidence { get; set; }
        }

        class ResultData
        {
            public List<Detection> detections { get; set; }
        }

        class Result
        {
            public ResultData data { get; set; }
        }

        public string Detect(string text)
        {
            var client = new RestClient("http://ws.detectlanguage.com");
            var request = new RestRequest("/0.2/detect", Method.POST);

            request.AddParameter("key", "f83eb972e3cab37ae54eac0d4bd24bd5"); // replace "demo" with your API key
            request.AddParameter("q", text);

            var response = client.Execute(request);
            var result = new JsonDeserializer().Deserialize<Result>(response);

            return result.data.detections[0].language;
        }
    }
}
