using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paranovels.DataModels;
using Paranovels.Facade;

namespace Paranovels.Tests.Paranovels.Facade
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var feedFacade = new FeedFacade();

            var i = feedFacade.CheckFeed(5);
        }
    }
}
