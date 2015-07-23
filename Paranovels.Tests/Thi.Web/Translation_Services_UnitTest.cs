using System;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thi.Web;

namespace Paranovels.Tests.Thi.Web
{
    [TestClass]
    public class Translation_Services_UnitTest
    {
        [TestMethod]
        public void Test_BabelFishTranslator()
        {
            var translator = new BabelFishTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "Want to be", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_BingTranslator()
        {
            var translator = new BingTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "Want to be", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_BaiduTranslator()
        {
            var translator = new BaiduTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "Want to be", true, "Not translating correctly.");

        }


        [TestMethod]
        public void Test_ExciteTranslator()
        {
            var translator = new ExciteTranslator();

            var originalText = "年間の歴史を振り返るさ";

            var translatedText = translator.Translate(originalText, "ja", "en");

            Assert.AreEqual(translatedText, "I look back to annual history.", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_GoogleTranslator()
        {
            var translator = new GoogleTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "Want to be", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_SystranetTranslator()
        {
            var translator = new SystranetTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "Wants into", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_LecTranslator()
        {
            var translator = new LecTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "Like to be", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_InfoSeekTranslator()
        {
            var translator = new InfoSeekTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "I hope that it becomes", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_BabylonTranslator()
        {
            var translator = new BabylonTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "You want to become a", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_HonyakuTranslator()
        {
            var translator = new HonyakuTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "I hope that it becomes", true, "Not translating correctly.");

        }

        [TestMethod]
        public void Test_FreeTranslationTranslator()
        {
            var translator = new FreeTranslationTranslator();

            var originalText = "想要成为";

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "You want to become a", true, "Not translating correctly.");

        }


        [TestMethod]
        public void Test_YoudaoTranslator()
        {
            var translator = new YoudaoTranslator();

            var originalText = "é¡µç¿»"; // this is unescape(encodeURICompenent(value)) from javascript

            var translatedText = translator.Translate(originalText, "zh", "en");

            Assert.AreEqual(translatedText, "page", true, "Not translating correctly.");

        }
    }
}
