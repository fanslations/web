using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using Thi.Core;

namespace Thi.Wpf.Selenium
{
    public static class SeleniumExtensions
    {
        public static void Select(this IWebElement element, string byValue)
        {
            var selectElement = new SelectElement(element);
            var by = byValue.Split('=').First() ?? "";
            var value = byValue.Split('=').Last() ?? "";
            if (by == "label")
            {
                selectElement.SelectByText(value);
            }
            else if (value.StartsWith("index"))
            {
                selectElement.SelectByIndex(int.Parse(value));
            }
            else if (value == "value")
            {
                selectElement.SelectByValue(value);
            }
            // click to set focus on the element
            element.Click();
        }

        public static void Type(this IWebElement element, string value)
        {
            element.Click();
          
            // erase any existing value (because clear does not send any events
            for (int i = 0; i < element.GetAttribute("value").Length; i++)
            {
                element.SendKeys(Keys.Backspace);
            }
            // type in value
            for (int i = 0; i < value.Length; i++)
            {
                element.SendKeys(value[i].ToString(CultureInfo.InvariantCulture));
            }
        }
    }
    public class SeRunner
    {

        public class SeInstruction
        {
            public string Command { get; set; }
            public string Target { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return string.Format("{0} <+> {1} <+> {2}", Command, Target, Value);
            }
        }
        public enum SeCommandeEnum
        {
            ClickAndWait,
            Click,

            Open,

            None,

            Pause,

            Select,

            Type
        }
        public enum SeTargetEnum
        {
            None,
            Css,
            Link,
            XPath,
            Id,

        }
        public enum SeBrowserDriver
        {
            Android,
            Chrome,
            Firefox,
            InternetExplorer,
            Safari,
        }
        private readonly IWebDriver _webDriver;
        private string _baseUrl;
        private string _seFilePath;

        public SeRunner(IWebDriver webDriver, string seFilePath)
        {
            _webDriver = webDriver;
            _seFilePath = seFilePath;
        }

        public void RunTest(string seFile)
        {
            var seInstructions = ParseSeFile();
            foreach (var seInstruction in seInstructions)
            {
                RunTest(seInstruction);
            }
        }

        public bool RunTestWithErrorReport(SeInstruction seInstruction)
        {
            bool passed;
            try
            {
                passed = RunTest(seInstruction);
            }
            catch (Exception ex)
            {
                var screenShot = _webDriver.TakeScreenshot();

                var email = new MailMessage("thi@domain.com", "nguye118@msu.edu", "Selenium Test Error", "A error occurred when running test case " + _seFilePath);
                email.Attachments.Add(new Attachment(new MemoryStream(screenShot.AsByteArray), seInstruction.ToString(), "image/png"));
                EmailService.Instance.Send(email);

                passed = false;
            }
            return passed;
        }

        public bool RunTest(SeInstruction seInstruction)
        {
            string targetType = "";
            string targetValue = "";

            // xpath=(//input[@name='Portal'])[3]
            if (seInstruction.Target.StartsWith("xpath="))
            {
                targetType = "xpath";
                targetValue = seInstruction.Target.Replace("xpath=","");
            }
            // //button[@type='submit']
            else if (seInstruction.Target.StartsWith("//"))
            {
                targetType = "xpath";
                targetValue = seInstruction.Target;
            }
            // /logout?type=manual&72=635911354550395204
            else if (seInstruction.Target.StartsWith("/"))
            {
                targetValue = seInstruction.Target;
            }
            // link=Home or id=Username
            else
            {
                var target = seInstruction.Target.Split('=');
                targetType = target[0];
                targetValue = target[1];
            }

            IWebElement element = null;
            SeTargetEnum t;
            Enum.TryParse(targetType, true, out t);
            switch (t)
            {
                case SeTargetEnum.None:
                    break;
                case SeTargetEnum.Css:
                    element = _webDriver.FindElement(By.CssSelector(targetValue));
                    break;
                case SeTargetEnum.Id:
                    element = _webDriver.FindElement(By.Id(targetValue));
                    break;
                case SeTargetEnum.Link:
                    element = _webDriver.FindElement(By.LinkText(targetValue));
                    break;
                case SeTargetEnum.XPath:
                    // xpath=(//input[@name='Portal'])[3]
                    if (targetValue.StartsWith("xpath"))
                    {
                        var xpath = targetValue.Substring(targetValue.IndexOf("(", StringComparison.Ordinal) + 1, targetValue.LastIndexOf(")", StringComparison.Ordinal) - targetValue.IndexOf("(", StringComparison.Ordinal) - 1);
                        var index = int.Parse(targetValue.Substring(targetValue.LastIndexOf("[", StringComparison.Ordinal) + 1, targetValue.LastIndexOf("]", StringComparison.Ordinal) - targetValue.LastIndexOf("[", StringComparison.Ordinal) - 1));
                        element = _webDriver.FindElements(By.XPath(xpath))[index - 1];
                    }
                    else
                    {
                        element = _webDriver.FindElement(By.XPath(targetValue));
                    }
                    break;
            }
            //if (element != null)
            //{
            //    _webDriver.ExecuteJavaScript<IWebElement>("arguments[0].setAttribute('value', 'Set to this text.');", element);
            //}
            // command
            SeCommandeEnum c;
            Enum.TryParse(seInstruction.Command, true, out c);
            switch (c)
            {
                case SeCommandeEnum.None:
                    break;
                case SeCommandeEnum.Click:
                    element.Click();
                    break;
                case SeCommandeEnum.ClickAndWait:
                    element.Click();
                    System.Threading.Thread.Sleep(1000);
                    break;
                case SeCommandeEnum.Open:
                    _webDriver.Navigate().GoToUrl(targetValue.StartsWith("http") ? targetValue : _baseUrl + targetValue);
                    System.Threading.Thread.Sleep(1000);
                    break;
                case SeCommandeEnum.Pause:
                    System.Threading.Thread.Sleep(int.Parse(targetValue));
                    break;
                case SeCommandeEnum.Select:
                    element.Select(seInstruction.Value);
                    break;
                case SeCommandeEnum.Type:
                    element.Type(seInstruction.Value);
                    break;
            }
            // click on body to get out of current action
            _webDriver.FindElement(By.TagName("body")).Click();
            return true;
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                _webDriver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private static String HtmlDecode(String s)
        {
            s = s.Replace("&quot;", "\"");
            s = s.Replace("&lt;", "<");
            s = s.Replace("&gt;", ">");
            s = s.Replace("&amp;", "&");
            s = s.Replace("&nbsp;", " ");
            return s;
        }

        public List<SeInstruction> ParseSeFile()
        {
            var testCase = new WebClient().DownloadString(_seFilePath);
            testCase = HtmlDecode(testCase);

            var match = Regex.Match(testCase, @"<link rel=""selenium.base"" href=""(?<baseurl>.+?)/"" />");
            if (match.Success)
            {
                _baseUrl = match.Groups["baseurl"].Value;
            }

            List<SeInstruction> result = new List<SeInstruction>();

            match = Regex.Match(testCase, @"<trs*[^>]*>\s*(<!--[\d\D]*?-->)?\s*<tds*[^>]*>\s*(?<command>[\w]*?)\s*</td>\s*<tds*[^>]*>(?<target>[\d\D]*?)</td>\s*(<tds*/>|<tds*[^>]*>(?<value>[\d\D]*?)</td>)\s*</tr>\s*");
            while (match.Success)
            {
                result.Add(new SeInstruction
                {
                    Command = match.Groups["command"].Value.Trim(),
                    Target = match.Groups["target"].Value.Replace("<br />", "\n").Trim(),
                    Value = match.Groups["value"].Value.Trim()
                });

                match = match.NextMatch();
            }
            return result;
        }
    }
}