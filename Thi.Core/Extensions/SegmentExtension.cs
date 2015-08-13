using System;
using System.Collections.Generic;

namespace Thi.Core
{
    public static class SegmentExtension
    {
        public static Dictionary<int, string> SegmentChapterContent(this string content)
        {
            var segments = new Dictionary<int, string>();
            var paragraphs = content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < paragraphs.Length; i++)
            {
                var paragraph = paragraphs[i].Trim();
                var hash = paragraph.GetIntHash();
                if (segments.ContainsKey(hash))
                {
                    var existParagraph = segments[hash];
                }
                else
                {
                    segments.Add(hash, paragraph);
                }
            }
            return segments;
        }
    }
}