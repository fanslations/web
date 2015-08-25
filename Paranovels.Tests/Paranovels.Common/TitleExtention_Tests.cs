using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paranovels.Common;

namespace Paranovels.Tests.Paranovels.Common
{
    [TestClass]
    public class TitleExtention_Tests
    {
        [TestMethod]
        public void Extract_chapter_from_title_test()
        {
            var titles = new Dictionary<string,string>
            {
                {"Death March kara Hajimaru Isekai Kyousoukyoku 8-22","8-22"},
                {"Monohito Chapter 15 – The Luminous Body’s Party Trick","15"},
                {"MTJ 16","16"},
                {"Fellow Daoists, please enjoy ISSTH Book 2, Chapter 119!!","119"},
                {"SM V3C3 Release","3"},
                {"WDQK Chapter 59: Killing the Panthers","59"},
                {"BTTH 146 and 147","146 and 147"},
                {"Child of Light Chapter Release – Volume 3: Chapter 26","26"},
                {"Stellar Transformation Book 12 Chapter 22","22"},
                {"Chaotic Sword God Chapter 88 - 89","88 - 89"},
                {"DED 88 + 89","88 + 89"},
                {"DED 88+89","88+89"},
                {"Doll Dungeon Chapter 5, 6,7 and 8","5, 6,7 and 8"},
                {"Doll Dungeon 23 & 24","23 & 24"},
                {"HHTP Chapters 23&24","23&24"},
                {"Death March kara Hajimaru Isekai Kyusoukyoku Chapter 7 Intermission 5","7 Intermission 5"},
                {"Magic, Mechanics, Shuraba Volume 1 Epilogue", "Epilogue"},
                {"Elqueeness’ Chapter-8. Summoned to the Human World Part 1","Chapter 8. Summoned to the Human World Part 1"}
            };

            foreach (var title in titles)
            {
                var chapter = title.Key.ExtractChapter().ToLower();

                Assert.IsTrue(chapter.Contains(title.Value.ToLower()), chapter + " != " + title.Value);

                Console.WriteLine(chapter);
            }
        }
    }
}
