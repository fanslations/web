using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Media;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Thi.Core;

namespace Thi.Wpf.Selenium
{
    public class TestLog
    {
        public TestCaseHtml TestCase { get; set; }
        public bool IsPassed { get; set; }
        public DateTime Date { get; set; }
    }

    public class TestLogService
    {
        readonly string _logFile = Path.Combine(Directory.GetCurrentDirectory(), "TestLog.json");
        public IList<TestLog> GetLogs()
        {
            if (!File.Exists(_logFile))
            {
                File.WriteAllText(_logFile, JsonHelper.Serialize(new List<TestLog>()), Encoding.UTF8);
            }
            return JsonHelper.Deserialize<IList<TestLog>>(File.ReadAllText(_logFile));
        }

        public void Save(TestCaseHtml testCase, bool isPassed)
        {
            var testLog = GetLogs();
            testLog.Add(new TestLog { TestCase = testCase, IsPassed = isPassed, Date = DateTime.Now });
            File.WriteAllText(_logFile, JsonHelper.Serialize(testLog));
        }
    }

    /// <summary>
    /// Interaction logic for PlayWindow.xaml
    /// </summary>
    public partial class PlayWindow : Window
    {
        private readonly MainWindow _parent;
        private SeRunner _seRunner;
        private IList<SeRunner.SeInstruction> _seInstructions;
        private IWebDriver _webDriver;
        private readonly TestCaseHtml _testCase;
        private int _i = 0;
        private TestLogService _testLogService;

        public PlayWindow(MainWindow parent, IWebDriver webDriver, TestCaseHtml testCase)
        {
            InitializeComponent();

            _parent = parent;
            _webDriver = webDriver;
            _testCase = testCase;

            _testLogService = new TestLogService();
        }



        public void Play()
        {
            _webDriver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 0, 0, 10));
            _seRunner = new SeRunner(_webDriver, _testCase.Url);
            _seInstructions = _seRunner.ParseSeFile();
            _seRunner.RunTest(_seInstructions[_i++]);
            StatusLabel.Text = "Click 'Next' to step through the story.";
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = false;
            RetryButton.IsEnabled = false;

            var passed = _seRunner.RunTestWithErrorReport(_seInstructions[_i++]);

            if (passed)
            {
                NextButton.IsEnabled = true;
                RetryButton.IsEnabled = true;
                StatusLabel.Text = string.Format("Passed {0} of {1}", _i, _seInstructions.Count);
                StatusLabel.Foreground = Brushes.DarkGreen;
            }
            else
            {
                NextButton.IsEnabled = false;
                RetryButton.IsEnabled = true;
                StatusLabel.Text = string.Format("Failed at {0}.", _i, _seInstructions.Count);
                StatusLabel.Foreground = Brushes.DarkRed;
            }

            if (_i == _seInstructions.Count)
            {
                NextButton.IsEnabled = false;
                RetryButton.IsEnabled = false;
                StatusLabel.Text = "Done. Click 'Return' button to go back.";
                _testLogService.Save(_testCase, passed);
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = _parent;
            this.Hide();
            _parent.RefreshWindow();
            _parent.Show();
        }

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            _i--;
            Next_Click(sender, e);
        }
    }
}
