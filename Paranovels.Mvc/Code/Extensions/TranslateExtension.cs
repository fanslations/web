using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paranovels.Mvc
{
    public static class TranslateExtension
    {
        public static string Translate<T>(this T key, Dictionary<T, string> translateData)
        {
            if (!ReferenceEquals(key, null) && translateData.ContainsKey(key))
                return translateData[key];
            return key as string;
        }

        public static string ToGoogleDriveImageUrl(this string fileID)
        {
            if (string.IsNullOrWhiteSpace(fileID))
                return "/assets/img/no-image.png";
            return "https://docs.google.com/uc?id=" + fileID;
        }
    }
}