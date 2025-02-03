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

namespace WebPulse
{
    public partial class MainWindow : Window
    {
        Settings settings = new Settings();

        private BrowserFetcher _browserFetcher;

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new Home();
 
            _browserFetcher = new BrowserFetcher();
            DownloadBrowserOnce();
            Start_Monitoring();
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

        private async Task<bool> SeeIfResourceExist(string url, string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                WebScraper webScraper = new WebScraper(_browserFetcher);
                return await webScraper.ScrapeWebsiteAsyncCode(url, code);
            }
            else
            {
                WebScraper webScraper = new WebScraper(_browserFetcher);
                return await webScraper.ScrapeWebsiteAsync(url);
            }
        }

        private async Task Monitoring_loop()
        {
            string path = "C:\\Users\\ahme1636\\OneDrive - Syddansk Erhvervsskole\\Dokumenter\\Webpulse\\WebPulse\\json\\SetupJson.json"; //Tried to make it relative, really did.
            
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                var objects = JsonConvert.DeserializeObject<List<MyObject>>(existingJson);

                while (true)
                {
                    foreach (var obj in objects)
                    {
                        bool exist = await SeeIfResourceExist(obj.Url, obj.Code);
                        if (exist)
                        {
                            MessageBox.Show("Resource exist");
                        }
                        else
                        {
                            MessageBox.Show("Resource does not exist");
                        }

                    await Task.Delay(10000);
                    }
                }
            }
        }
    }
}
