using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Web
{
    public static class WebClientFactory
    {
        public static WebClient ChromeClient()
        {
            ServicePointManager.Expect100Continue = false; // remove "Expect: 100-continue" header
            var webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36");
            webClient.Headers.Add("X-Requested-With", "XMLHttpRequest");
            return webClient;
        }
    }
}
