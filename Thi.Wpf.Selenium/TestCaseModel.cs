using System.Collections.Generic;

namespace Thi.Wpf.Selenium
{
    public class TestCaseModel
    {
        public string BaseUrl { get; set; }
        public IList<TestCaseHtml> TestCases { get; set; }
    }
}