using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Newtonsoft.Json;
using PuppeteerSharp;
using System.Diagnostics;
using WebPulse;
using System.Collections.Generic;

namespace WebPulse
{
    public partial class MainWindow : Window
    {
        Settings settings = new Settings();
        private BrowserFetcher _browserFetcher;
        private IBrowser _browser;
        private HelperCode helperCode;

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new Home();
            helperCode = new HelperCode();
            _browserFetcher = new BrowserFetcher();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await DownloadBrowserOnce();
            await LaunchBrowserOnce();
            Start_Monitoring();
            Debug.WriteLine("Starting");
        }

        private async void Start_Monitoring()
        {
            await Monitoring_loop();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Home();
        }

        private void Monitor_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Monitor();
        }

        public void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Settings();
        }

        public void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private async Task DownloadBrowserOnce()
        {
            await _browserFetcher.DownloadAsync();
        }

        private async Task LaunchBrowserOnce()
        {
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        }

        private async Task<bool> LookForRelease(MyObject myObject)
        {
            WebScraper webScraper = new WebScraper(_browser);
            if (myObject.Method == "urlbased")
            {
                string updatedUrl = UpdateUrl(myObject.Url, myObject.Count);
                Debug.WriteLine(updatedUrl);
                return await webScraper.ScrapeWebsiteAsync(updatedUrl);
            }
            else if (myObject.Method == "codebased")
            {
                return await webScraper.ScrapeWebsiteAsyncCode(myObject.Url, myObject.Code);
            }
            else
            {
                return false;
            }
        }

        private string UpdateUrl(string url, int count)
        {
            return Regex.Replace(url, @"\*(\d+)\*", match => (count + 1).ToString());
        }

        private async Task Monitoring_loop()
        {
            string path = helperCode.GetJsonLocation();
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                var objects = JsonConvert.DeserializeObject<System.Collections.Generic.List<MyObject>>(existingJson);

                var queue = new SortedList<DateTime, MyObject>();
                foreach (var obj in objects)
                {
                    queue.Add(DateTime.Now.AddMilliseconds(ConvertToMilliseconds(obj.Refresh, obj.TimeUnit)), obj);
                }

                while (true)
                {
                    if (queue.Count == 0)
                        break;

                    var first = queue.Keys[0];
                    var obj = queue[first];
                    queue.RemoveAt(0);

                    var delay = (int)(first - DateTime.Now).TotalMilliseconds;
                    if (delay > 0)
                        await Task.Delay(delay);

                    bool release = await LookForRelease(obj);
                    if (release)
                    {
                        Debug.WriteLine("Resource exists");
                    }

                    queue.Add(DateTime.Now.AddMilliseconds(ConvertToMilliseconds(obj.Refresh, obj.TimeUnit)), obj);
                }
            }
        }

        private int ConvertToMilliseconds(int refresh, string timeUnit)
        {
            return timeUnit switch
            {
                "Minutes" => refresh * 60 * 1000,
                "Hours" => refresh * 60 * 60 * 1000,
                "Days" => refresh * 24 * 60 * 60 * 1000,
                "Weeks" => refresh * 7 * 24 * 60 * 60 * 1000,
                _ => 60 * 1000 // Default to 1 minute if invalid
            };
        }

        protected override async void OnClosed(EventArgs e)
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
            base.OnClosed(e);
        }
    }
}
