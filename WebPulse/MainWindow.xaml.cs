using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

using WebPulse.models;

namespace WebPulse
{
    public partial class MainWindow : Window
    {
        #region Variables and Constructor

        private readonly BrowserFetcher _browserFetcher;
        private IBrowser _browser;
        private readonly HelperCode _helperCode;

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new Home();
            _helperCode = new HelperCode();
            _browserFetcher = new BrowserFetcher();
            Loaded += MainWindow_Loaded;
        }


        #endregion
        #region MainLogic








        private void SaveRelease(Dictionary<string, string> configItems) //Copy of the method used for putting setups some names might not be most suitable.
        {
            try
            {
                string path = _helperCode.GetReleaseJson();
                Debug.WriteLine("File path: " + path);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                List<Dictionary<string, string>> jsonList = new List<Dictionary<string, string>>();

                if (File.Exists(path))
                {
                    string existingJson = File.ReadAllText(path);
                    jsonList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(existingJson);
                }

                jsonList.Add(new Dictionary<string, string>(configItems));

                string json = JsonConvert.SerializeObject(jsonList, Formatting.Indented);

                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the setup configuration: " + ex.Message);
                Debug.WriteLine("Error: " + ex.Message);
            }
        }
        
        private async Task<bool> LookForRelease(MyObject myObject)
        {
            WebScraper webScraper = new WebScraper(_browser);
            
            if (myObject.Method == "urlbased")
            {
                string updatedUrl = UpdateUrl(myObject.Url, int.Parse(myObject.Count));
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
        private async Task Monitoring_loop()
        {
            try
            {
                Debug.WriteLine("Starting Monitoring Loop...");
                var objects = _helperCode.GetSetupJsonObjects();
                var queue = new SortedList<DateTime, MyObject>();
                foreach (var obj in objects)
                {
                    if (obj.Enabled == "true")
                    {
                        try
                        {
                            var nextRunTime = DateTime.Now.AddMilliseconds(ConvertToMilliseconds(int.Parse(obj.Refresh), obj.TimeUnit));
                            queue.Add(nextRunTime, obj);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error adding object to queue: {ex.Message}");
                        }
                    }
                }
                while (true)
                {
                    if (queue.Count == 0)
                    {
                        Debug.WriteLine("Queue is empty. Stopping loop.");
                        break;
                    }

                    var first = queue.Keys[0];
                    var obj = queue[first];
                    queue.RemoveAt(0);

                    var delay = (int)(first - DateTime.Now).TotalMilliseconds;
                    if (delay > 0)
                    {
                        Debug.WriteLine($"Next search is in {delay / 1000.0:F1} seconds");
                        await Task.Delay(delay);
                    }

                    try
                    {
                        Debug.WriteLine("Checking for resource...");
                        bool release = await LookForRelease(obj);

                        if (release)
                        {
                            Debug.WriteLine("Resource exists!");
                            //SaveRelease();
                            //logic for in-app notification
                            //logic for in-desktop notification
                        }
                        else
                        {
                            Debug.WriteLine("No resource found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error during resource check: {ex.Message}");
                    }

                    try
                    {
                        var nextRunTime = DateTime.Now.AddMilliseconds(ConvertToMilliseconds(int.Parse(obj.Refresh), obj.TimeUnit));
                        queue.Add(nextRunTime, obj);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error re-adding object to queue: {ex.Message}");
                    }
                }

                Debug.WriteLine("Monitoring loop ended.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Critical error in Monitoring_loop: {ex.Message}");
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        #endregion
        #region Navigationlevel

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Home();
        }
        
        private void Monitor_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Monitor();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Settings();
        }
        
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
        
        private void MinimizeApp(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void MaximizeApp(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        private void CloseApp(object sender, RoutedEventArgs e) => Close();
        
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        
        protected override async void OnClosed(EventArgs e)
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
            base.OnClosed(e);
        }
        
        #endregion
        #region DoOnce
        
        private async Task DownloadBrowserOnce()
        {
            await _browserFetcher.DownloadAsync();
        }
        private async Task LaunchBrowserOnce()
        {
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
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
        
        
        
        

        #endregion
        #region Utility
        
        private string UpdateUrl(string url, int count)
        {
            return Regex.Replace(url, @"\*(\d+)\*", match => (count + 1).ToString());
        }
        private int ConvertToMilliseconds(int refresh, string timeUnit)
        {
            return timeUnit switch
            {
                "Minutes" => refresh * 60 * 1000,
                "Hours" => refresh * 60 * 60 * 1000,
                "Days" => refresh * 24 * 60 * 60 * 1000,
                "Weeks" => refresh * 7 * 24 * 60 * 60 * 1000,
                _ => 60 * 1000 
            };
        }        

        #endregion
        
    }
}
