using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Tests.Thi.Core
{
    [TestClass]
    public class PropertyMapper_UnitTest
    {
        [TestMethod]
        public void Test_mapping_property_between_two_object()
        {
            var novel = new Novel();
            var novelForm = new NovelForm
            {
                Title = "test title",
                Synopsis = "test sysnopsis"
            };

            new PropertyMapper<NovelForm, Novel>(novelForm, novel).Map(null);

            Assert.AreEqual(novelForm.Title, novel.Title);
            Assert.AreEqual(novel.CompletedDate, DefaultValue.DateTime);
        }
    }
}
