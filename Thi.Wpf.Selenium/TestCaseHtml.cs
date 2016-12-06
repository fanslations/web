using System;

namespace Thi.Wpf.Selenium
{
    public class TestCaseHtml
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public bool IsPassed { get; set; }
        public DateTime TestedDate { get; set; }
    }
}