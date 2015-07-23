using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paranovels.Proxies;

namespace Paranovels.Tests.Paranovels.Proxies
{
    [TestClass]
    public class FeedChecker_UnitTest
    {
        [TestMethod]
        public void WordPress_feed_test()
        {
            var uc = new FeedChecker();
            var chapters = uc.Check("http://arkmachinetranslations.com/feed/");

            Assert.IsTrue(chapters.Any());
        }

        [TestMethod]
        public void BlogSpot_feed_test()
        {
            var uc = new FeedChecker();
            var chapters = uc.Check("http://clickyclicktranslation.blogspot.com/feeds/posts/default?alt=rss");

            Assert.IsTrue(chapters.Any());
        }
    }
}

