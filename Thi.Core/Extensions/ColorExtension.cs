using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public static class ColorExtension
    {
        public static string ToColorHex(this int colorInt)
        {
            var hexColor = "#" + Convert.ToString(colorInt, 16).PadLeft(6,'0');
            return hexColor;
        }
        public static int ToColorInt(this string colorHex)
        {
            var intColor = Convert.ToInt32(colorHex.Replace("#",""), 16);
            return intColor;
        }
    }
}
