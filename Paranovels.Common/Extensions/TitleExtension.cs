using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Paranovels.Common
{
    public static class TitleExtension
    {
        public static string ExtractVolumeChapter(this string title)
        {
            if (IsChapterRelease(title))
                return (ExtractVolume(title) + " " + ExtractChapter(title)).Trim();
            return title;
        }

        public static string ExtractVolume(this string title)
        {
            var volume = "";
            // get volume/book
            var match = Regex.Match(title, @"(v|vol|volume|b|book)\s?(?<v>\d+)", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            if (match.Success)
            {
                if (match.Value.Contains("b"))
                {
                    volume = "Book " + match.Groups["v"].Value;
                }
                else
                {
                    volume = "Volume " + match.Groups["v"].Value;
                }

            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(volume);
        }

        public static string ExtractChapter(this string title)
        {
            if (!IsChapterRelease(title)) return "(Not a Chapter)";
            
            var chapter = "";
            // get volume/book
            var match = Regex.Match(title, @"(c|ch|chap|chapter|chapters)\s?(?<c>(\d+([a-z-,–\.\&\+\s]*\d+)*)+)", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            if (match.Success)
            {
                if (match.Value.Contains("s"))
                {
                    chapter = "Chapters " + match.Groups["c"].Value;
                }
                else
                {
                    chapter = "Chapter " + match.Groups["c"].Value;
                }
            }
            else if (title.EndsWith("Epilogue", StringComparison.OrdinalIgnoreCase))
            {
                chapter = "Epilogue";
            }
            else
            {
                match = Regex.Match(title, @"(?<c>(\d+([a-z-,–\.\&\+\s]*\d+)*)+)", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    chapter = "Chapter " + match.Groups["c"].Value;
                }
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(chapter);
        }

        public static bool IsChapterRelease(this string title)
        {
            var isNotChapter = false;
            title = title.ToLower();

            isNotChapter = title.Contains("updates") || title.Contains("update post") || title.Contains("not a chapter")
                || Regex.Match(title, @"(?<c>(\d+([a-z-,–\.\&\+\s]*\d+)*)+)", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase).Success == false;

            return isNotChapter == false;
        }
    }
}