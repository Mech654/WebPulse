using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using PuppeteerSharp;
using Newtonsoft.Json;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using WebPulse;
using System.Diagnostics;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace WebPulse
{
    public partial class MainWindow : Window
    {
        Settings settings = new Settings();

        private BrowserFetcher _browserFetcher;

        private HelperCode helperCode;

        public MainWindow()
        {
            this.helperCode = new HelperCode();

            InitializeComponent();
            MainContent.Content = new Home();
 
            _browserFetcher = new BrowserFetcher();
            DownloadBrowserOnce();
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

        private async void DownloadBrowserOnce()
        {
            await _browserFetcher.DownloadAsync();
        }

        private async Task<bool> SeeIfResourceExist(MyObject setup)
        {
            if (setup.Method == "urlbased")
            {
                Debug.WriteLine("URL based");
                WebScraper webScraper = new WebScraper(_browserFetcher);
                return await webScraper.ScrapeWebsiteAsync(setup.Url);
            }
            else if (setup.Method == "codebased")
            {
                WebScraper webScraper = new WebScraper(_browserFetcher);
                return await webScraper.ScrapeWebsiteAsyncCode(setup.Url, setup.Code);
            }
            else
            {
                return false;
            }
        }


        private async Task<bool> LookForRelease(MyObject myObject)
        {
            if (myObject.Method == "urlbased")
            {
                WebScraper webScraper = new WebScraper(_browserFetcher);
                Debug.WriteLine(UpdateUrl(myObject.Url, myObject.Count));
                return await webScraper.ScrapeWebsiteAsync(UpdateUrl(myObject.Url, myObject.Count));
            }
            else if (myObject.Method == "codebased")
            {
                WebScraper webScraper = new WebScraper(_browserFetcher);
                return await webScraper.ScrapeWebsiteAsyncCode(myObject.Url, myObject.Code);
            }
            else
            {
                return false;
            }
        }

        private string UpdateUrl(string url, int count)
        {
            return Regex.Replace(url, @"\*(\d+)\*", (match) => (count + 1).ToString());
        }



        private async Task Monitoring_loop()
        {
            string path = helperCode.GetJsonLocation();
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                var objects = JsonConvert.DeserializeObject<List<MyObject>>(existingJson);

                while (true)
                {
                    foreach (var obj in objects)
                    {
                        bool exist = await SeeIfResourceExist(obj);
                        if (exist)
                        {
                            bool release = await LookForRelease(obj);
                            if (release)
                            {
                                Debug.WriteLine("Resource exist");
                            }
                            else
                            {
                                //MessageBox.Show("Resource does not exist");
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Resource does not exist");
                        }

                    }
                    await Task.Delay(1000);
                }
            }
        }
    }
}
