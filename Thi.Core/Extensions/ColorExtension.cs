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
            return "#" + Convert.ToString(colorInt, 16);
        }
        public static int ToColorInt(this string colorHex)
        {
            return Convert.ToInt32(colorHex.Replace("#",""), 16);
        }
    }
}
