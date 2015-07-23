using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public static class FormatExtension
    {
        public static string FormatDate(this DateTime value, string blankReplacement = "blank")
        {
            if (value == DefaultValue.DateTime) return "<i class='blank'>" + blankReplacement + "</i>";
            return value.ToString("MM/dd/yyyy");
        }

        public static string FormatInt(this int value, string blankReplacement = "blank")
        {
            if (value == DefaultValue.Int) return "<i class='blank'>" + blankReplacement + "</i>";
            return value.ToString("##,###");
        }

        public static string FormatString(this string value, int maxLength = 0, int buffer = 20, string blankReplacement = "blank")
        {
            if (string.IsNullOrWhiteSpace(value)) return "<i class='blank'>" + blankReplacement + "</i>";
            if (value.Length > maxLength + buffer && maxLength > 0)
            {
                maxLength = value.IndexOfAny(new []{' ', ',', '.'}, maxLength);
                var firstPart = value.Substring(0, maxLength);
                var lastPart = value.Substring(maxLength);

                var html = @"<span id=""{2}"" class=""more-less"" onclick=""$('#{2}').toggleClass('on')"">{0}<a  class=""more"">...</a><span class=""more-info"">{1}</span></span>";
                return string.Format(html, firstPart, lastPart, "more-less" + value.GetIntHash());
            }
            return value;
        }
    }
}
