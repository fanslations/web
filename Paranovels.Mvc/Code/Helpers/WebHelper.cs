using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Drive.v2.Data;

namespace Paranovels.Mvc
{
    public static class WebHelper
    {
        public static string GetClientIpAddress()
        {
            var serverVariables = HttpContext.Current.Request.ServerVariables;
            return serverVariables["HTTP_X_FORWARDED_FOR"] ?? serverVariables["X_FORWARDED_FOR"] ?? serverVariables["REMOTE_ADDR"];
        }

        public static string GetClientDevice()
        {
            var devices = new string[]
            {
                "iPhone", "iPad", "iPod", "BlackBerry",
                "Nokia", "Android", "WindowsPhone",
                "Mobile"
            };

            var userAgent = HttpContext.Current.Request.UserAgent ?? "";

            if (devices.Any(userAgent.Contains))
            {
                return "mobile";
            }
            return "desktop";
        }
    }
}