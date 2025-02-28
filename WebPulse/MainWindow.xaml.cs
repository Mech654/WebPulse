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
            DarkMode.Enable();
        }


        #endregion

        #region MainLogic








        private void SaveRelease(string currentTime, string objName, string updatedUrl, string number, string title)
        {
            MyReleases release = new MyReleases
            {
                Name = objName,
                Time = currentTime,
                Link = updatedUrl,
                Title = title,
                Number = number
            };

            try
            {
                string path = _helperCode.GetReleaseJson();
                Debug.WriteLine("File path: " + path);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Check if the file exists
                if (File.Exists(path))
                {
                    string existingJson = File.ReadAllText(path);

                    // Deserialize existing JSON into a list
                    var releaseList = JsonConvert.DeserializeObject<List<MyReleases>>(existingJson) ??
                                      new List<MyReleases>();

                    // Add the new release to the list
                    releaseList.Add(release);

                    // Serialize the updated list back to JSON
                    string json = JsonConvert.SerializeObject(releaseList, Formatting.Indented);

                    // Write the updated JSON back to the file
                    File.WriteAllText(path, json);
                }
                else
                {
                    // If file does not exist, create a new list and add the release
                    var releaseList = new List<MyReleases> { release };
                    string json = JsonConvert.SerializeObject(releaseList, Formatting.Indented);
                    File.WriteAllText(path, json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the setup configuration: " + ex.Message);
                Debug.WriteLine("Error: " + ex.Message);
            }
        }




        private async Task<(string, bool, string)> LookForRelease(MyObject myObject)
        {
            WebScraper webScraper = new WebScraper(_browser);

            if (myObject.Method == "urlbased")
            {
                (bool result, string title) = await webScraper.ScrapeWebsiteAsync(UpdateUrl(myObject.Url, int.Parse(myObject.Count)));
                string updatedUrl = UpdateUrlWithStars(myObject.Url, int.Parse(myObject.Count));
                return (updatedUrl, result, title); // Return updated URL with * * and success (true)
            }
            else if (myObject.Method == "codebased")
            {
                ( bool result, string url, string title) = await webScraper.ScrapeWebsiteAsyncCode(myObject.Url, myObject.Code);
                return (url, result, title); // Return the URL and the result of the scrape (true/false)
            }
            else
            {
                return ("", false, ""); // Return an empty string and false if method is not recognized
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
                            var nextRunTime =
                                DateTime.Now.AddMilliseconds(
                                    ConvertToMilliseconds(int.Parse(obj.Refresh), obj.TimeUnit));
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
                    var currentObj = queue[first];
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
                        (string updatedUrl, bool isSuccess, string title) = await LookForRelease(currentObj);

                        if (isSuccess)
                        {
                            string currentTime = DateTime.Now.ToString();
                            Debug.WriteLine("Resource exists!");

                            SaveRelease(currentTime, currentObj.Name, RemoveAsterisks(updatedUrl), currentObj.Count, title);
                            _helperCode.UpdateSetupJsonValue(currentObj.Name, "Url", updatedUrl);
                            _helperCode.IncrementSetupJsonCount(currentObj.Name);
                            // logic for in-app notification
                            // logic for in-desktop notification

                            // Refresh the JSON objects and rebuild the queue.
                            objects = _helperCode.GetSetupJsonObjects();
                            queue.Clear();
                            foreach (var obj in objects)
                            {
                                if (obj.Enabled == "true")
                                {
                                    try
                                    {
                                        var nextRunTime =
                                            DateTime.Now.AddMilliseconds(ConvertToMilliseconds(int.Parse(obj.Refresh),
                                                obj.TimeUnit));
                                        queue.Add(nextRunTime, obj);
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Error adding object to queue: {ex.Message}");
                                    }
                                }
                            }
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
                        var nextRunTime =
                            DateTime.Now.AddMilliseconds(ConvertToMilliseconds(int.Parse(currentObj.Refresh),
                                currentObj.TimeUnit));
                        queue.Add(nextRunTime, currentObj);
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
            MainContent.Content = new Settings(this);
        }

        public void Refresh_Settings()
        {
            MainContent.Content = new Settings(this);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void MinimizeApp(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeApp(object sender, RoutedEventArgs e) => WindowState =
            WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

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

        public static string RemoveAsterisks(string input)
        {
            return Regex.Replace(input, @"\*(\d+)\*", "$1");
        }

        private string UpdateUrl(string url, int count)
        {
            return Regex.Replace(url, @"\*(\d+)\*", match => (count + 1).ToString());
        }

        private string UpdateUrlWithStars(string url, int count)
        {
            return Regex.Replace(url, @"\*(\d+)\*", match => $"*{count + 1}*");
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

