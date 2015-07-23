using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thi.Core;

namespace Paranovels.Tests.Thi.Core
{
    [TestClass]
    public class StringExtension_UnitTest
    {
        [TestMethod]
        public void GetMd5Hash_Test()
        {
            const string str = "lsdk skldjflsflksdfj slsdjflksdjflksdf"; // A7C7B3E44069055598ED4E827390E845

            const string str2 = "http://onlinemd5.com/"; // 11CAA606AB25AEB61F7384D2BF016DEC

            var md5Hash = str.GetMd5Hash();
            Assert.AreEqual(md5Hash, "A7C7B3E44069055598ED4E827390E845", true);

            var md5Hash2 = str2.GetMd5Hash();
            Assert.AreEqual(md5Hash2, "11CAA606AB25AEB61F7384D2BF016DEC", true);
        }

        [TestMethod]
        public void GetIntHash_Test()
        {
            const string str = "tEst"; // A7C7B3E44069055598ED4E827390E845

            const string str2 = "Test"; // 11CAA606AB25AEB61F7384D2BF016DEC

            Assert.AreEqual(str.GetMd5Hash(), str2.GetMd5Hash());

            Assert.AreNotEqual(str.GetMd5Hash(false), str2.GetMd5Hash());

            Assert.AreEqual(str.GetIntHash(), str2.GetIntHash());

            Assert.AreNotEqual(str.GetIntHash(false), str2.GetIntHash());

            Console.WriteLine("{0} - {1} : {2} - {3}", str.GetMd5Hash(), str2.GetMd5Hash(), str.GetIntHash(), str2.GetIntHash());
        }
    }
}
