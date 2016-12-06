using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Thi.Core;
using Brushes = System.Drawing.Brushes;

namespace Thi.Wpf.Selenium
{
    public static class Converter
    {
        public static readonly IValueConverter RedGreenColor = new RedGreenColorConverter();
    }
    public class RedGreenColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = (bool)value;
            if (b)
                return new SolidColorBrush(Colors.DarkGreen);
            return new SolidColorBrush(Colors.DarkRed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TestCaseChecker : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var testLogs = new TestLogService().GetLogs();
            var testLog = testLogs.Where(w => w.TestCase.Url == value.ToString()).OrderByDescending(o => o.Date).FirstOrDefault();

            if (testLog == null)
            {
                return "N/A";
            }
            else
            {
                if (testLog.IsPassed)
                {
                    return "✓ " + testLog.Date; ;
                }
                else
                {
                    return "Ⓔ " + testLog.Date;
                }

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Browser
    {
        public string Name { get; set; }
        public int WorkingLevel { get; set; }
    }
    public class MainModel
    {
        public ObservableCollection<TestCaseHtml> TestCases { get; set; }
        public IList<Browser> Browsers { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IWebDriver _webDriver = null;
        private MainModel _mainModel;
        private Browser _selectedBrowser;
        private TestCaseHtml _selectedTestCase;

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += OnClosing;
            this.Loaded += OnLoaded;
            this.DataContextChanged += OnDataContextChanged;

            RefreshWindow();

            this.DataContext = _mainModel;
        }

        public void RefreshWindow()
        {
            var json = new WebClient().DownloadString("http://localhost:8000/testcase/index");
            var model = JsonHelper.Deserialize<TestCaseModel>(json);
            var testLogs = new TestLogService().GetLogs();

            _mainModel = new MainModel()
            {
                TestCases = new ObservableCollection<TestCaseHtml>(model.TestCases.OrderByDescending(o => o.Date).ThenBy(o => o.Name).Select(s =>
                {
                    var testLog = testLogs.OrderByDescending(o => o.Date).FirstOrDefault(w => w.TestCase.Url == s.Url);
                    if (testLog != null)
                    {
                        s.IsPassed = testLog.IsPassed;
                        s.TestedDate = testLog.Date;
                    }
                    return s;
                })),
                Browsers = new[]
                {
                   new Browser { Name   = "Firefox", WorkingLevel = 1 },
                   new Browser { Name   = "Chrome", WorkingLevel = 2 },
                   //new Browser { Name   = "Edge", WorkingLevel = 9 },
                   new Browser { Name   = "Internet Explorer", WorkingLevel = 9 },
                },
            };
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            StoryList.SelectedItem = _selectedTestCase;
            BrowserList.SelectedItem = _selectedBrowser;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {

        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (_webDriver != null)
            {
                _webDriver.Quit();
                _webDriver = null;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _selectedTestCase = StoryList.SelectedItem as TestCaseHtml;
            _selectedBrowser = BrowserList.SelectedItem as Browser;
            if (_selectedBrowser == null || _selectedTestCase == null)
            {
                MessageBox.Show("Select a browser and test case above then click play.");
                return;
            }
            
            switch (_selectedBrowser.Name)
            {
                case "Chrome":
                    if (_webDriver == null || _webDriver.GetType() != typeof(ChromeDriver))
                    {
                        if(_webDriver != null) _webDriver.Quit();
                        _webDriver = new ChromeDriver();
                    }
                    break;
                case "Edge":
                    if (_webDriver == null || _webDriver.GetType() != typeof(EdgeDriver))
                    {
                        if (_webDriver != null) _webDriver.Quit();
                        _webDriver = new EdgeDriver();
                    }
                    break;
                case "Firefox":
                    if (_webDriver == null || _webDriver.GetType() != typeof(FirefoxDriver))
                    {
                        if (_webDriver != null) _webDriver.Quit();
                        _webDriver = new FirefoxDriver();
                    }
                    break;
                case "Internet Explorer":
                    if (_webDriver == null || _webDriver.GetType() != typeof(InternetExplorerDriver))
                    {
                        if (_webDriver != null) _webDriver.Quit();
                        InternetExplorerOptions internetExplorerOptions = new InternetExplorerOptions
                        {
                            IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        };
                        _webDriver = new InternetExplorerDriver(internetExplorerOptions);
                    }
                    break;
                default:
                    _webDriver = new FirefoxDriver();
                    break;
            }

            var playWindow = new PlayWindow(this, _webDriver, _selectedTestCase);
            playWindow.Play();

            Application.Current.MainWindow = playWindow;
            this.Hide();
            playWindow.Show();
        }
    }
}
