using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Thi.Core
{
    public class SimpleTemplate
    {
        object Entity { get; set; }
        StringBuilder Builder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapper"/> class.
        /// </summary>
        /// <param name="entity">The entity(object or dictionary).</param>
        public SimpleTemplate(object entity)
        {
            this.Entity = entity;
            this.Builder = new StringBuilder();
        }

        public string Process(string content)
        {
            if (Entity == null) return content;

            const string pattern = "{{(?'property'\\S+?)}}";
            var mappedContent = Regex.Replace(content, pattern, new MatchEvaluator(this.RegexMatchEvaluation), RegexOptions.ExplicitCapture);
            if (Regex.IsMatch(mappedContent, pattern))
            {
                return mappedContent == content ? mappedContent : Process(mappedContent);
            }
            return mappedContent;
        }

        private string RegexMatchEvaluation(Match match)
        {
            // handle Dictionary type
            if (Entity.GetType() == typeof(Dictionary<string, string>) || Entity.GetType() == typeof(Dictionary<string, object>))
            {
                return ((Dictionary<string, string>)Entity).ContainsKey(match.Groups["property"].Value)
                           ? ((Dictionary<string, string>)Entity)[match.Groups["property"].Value]
                           : match.Value;
            }
            // handle object type
            PropertyInfo property = this.Entity.GetType().GetProperty(match.Groups["property"].Value);
            if (property == null)
                return match.Value;
            object obj = property.GetValue(this.Entity, (object[])null);
            if (obj != null)
                return obj.ToString();

            return match.Value;
        }
    }
}
