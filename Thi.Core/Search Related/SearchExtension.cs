using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace Thi.Core
{
    public static class SearchExtension
    {
        public static string[] SplitReservedQuote(this string value)
        {
            var splitted = value.Split('"').Select((element, index) => index % 2 == 0  // If even index
                                                                       ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                                                       : new string[] { element })  // Keep the entire item
                                           .SelectMany(element => element).ToArray();
            return splitted;
        }

        public static string Peek(this string value, int maxCharacters = 200)
        {
            var length = value.Length > maxCharacters ? maxCharacters : value.Length;
            return value.Substring(0, length);
        }

        public static string Abbr(this string value, int maxCharacters = 20)
        {
            if (value.Length > maxCharacters)
                return value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Aggregate("", (current, word) => current + word.Substring(0, 1).ToUpper());
            return value;
        }

        public static string[] ToSearchKeywords(this string query)
        {
            return query.ToLower().SplitReservedQuote()
                .Where(w => !w.Contains("=")).Distinct().ToArray(); // remove mapped keyword
        }

        static object CastTo(string value, string typeName)
        {
            if (typeName.Contains("System.Int32"))
            {
                if (typeName.Contains("IList"))
                {
                    return new List<int>(value.Split(',').Select(int.Parse));
                }
                return int.Parse(value);
            }

            if (typeName.Contains("System.String"))
            {
                if (typeName.Contains("IList"))
                {
                    return new List<string>(value.Split(','));
                }
                return value;
            }

            if (typeName.Contains("System.Boolean"))
            {
                const string trueValue = "on,true,yes,checked";
                if (trueValue.Contains(value.ToLower()))
                    return true;

                const string falseValue = "off,false,no,unchecked";
                if (falseValue.Contains(value.ToLower()))
                    return false;

                return null;
            }

            if (typeName.Contains("System.DateTime"))
            {
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                return DateTime.Parse(value);
            }

            return value;

        }

        public static T ToCriteria<T>(this NameValueCollection nvc) where T : ICriteriaModel, new()
        {
            var criteria = new T();
            var type = typeof(T);

            // simple search
            if (!string.IsNullOrWhiteSpace(nvc["query"]))
            {
                criteria.SearchType = "Simple";
                criteria.Query = nvc["query"];
                // translate special value like ssn etc...


                // search with specific value
                var keywords = criteria.Query.SplitReservedQuote();
                foreach (var keyword in keywords)
                {
                    var keyValue = keyword.Split(new[] { ":", "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (keyValue.Length == 2)
                    {
                        if (keyValue[0] != "")
                        {
                            var property = type.GetProperty(keyValue[0],
                                                            BindingFlags.IgnoreCase |
                                                            BindingFlags.Public |
                                                            BindingFlags.Instance);
                            if (property != null && property.CanWrite)
                            {
                                property.SetValue(criteria, CastTo(keyValue[1], property.PropertyType.FullName), null);
                            }
                        }
                    }
                }
            }
            // advance search
            for (int i = 0; i < nvc.Count; i++)
            {
                var key = nvc.GetKey(i);
                // skip empty key
                if (string.IsNullOrEmpty(key) || key == "query") continue;

                var property = type.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null && property.CanWrite)
                {
                    if (!string.IsNullOrWhiteSpace(nvc.Get(i)))
                    {
                        property.SetValue(criteria, CastTo(nvc.Get(i), property.PropertyType.FullName), null);
                        criteria.SearchType = "Advance";
                    }
                }
            }
            // screen mode
            if (nvc["sm"] != null)
            {
                criteria.HiddenFields = criteria.HiddenFields ?? new Dictionary<string, string>();
                criteria.HiddenFields.Add("sm", nvc["sm"]);
            }
            return criteria;
        }

    }
}