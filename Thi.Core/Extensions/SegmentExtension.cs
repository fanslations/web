using System;
using System.Collections.Generic;
using System.Linq;

namespace Thi.Core
{
    public static class SegmentExtension
    {
        public static Dictionary<int, string> SegmentChapterContent(this string content)
        {
            var segments = new Dictionary<int, string>();
            var paragraphs = ToParagraphs(content);
            foreach (string paragraph in paragraphs)
            {
                var hash = paragraph.GetIntHash();
                if (!segments.ContainsKey(hash))
                {
                    segments.Add(hash, paragraph);
                }
            }
            return segments;
        }

        public static List<string> ToParagraphs(this string content)
        {
            var paragraphs = content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return paragraphs.Where(w=> !string.IsNullOrWhiteSpace(w)).Select(s=> s.Trim()).ToList();
        } 
    }
}