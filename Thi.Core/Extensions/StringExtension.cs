using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public static class StringExtension
    {
        public static bool Contains(this string source, string toCheck, StringComparison comparisonType)
        {
            return source.IndexOf(toCheck, comparisonType) >= 0;
        }

        public static string GetMd5Hash(this string input, bool ignoreCase = true)
        {
            if (ignoreCase) input = input.Trim().ToLower();

            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static int GetIntHash(this string input, bool ignoreCase = true)
        {
            if(ignoreCase) input = input.Trim().ToLower();

            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            var intHash = 1;
            for (int i = 0; i < data.Length; i++)
            {
                intHash += (intHash * Convert.ToInt32(data[i]) * (i + 1)) % int.MaxValue;
            }
            return intHash;
        }
    }
}
