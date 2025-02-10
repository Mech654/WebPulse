using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WebPulse
{
    public partial class Monitor : UserControl
    {
        private readonly HelperCode helperCode;
        private ListInitializer _listInitializer;
        private Dictionary<string, string> SetupConfigItems { get; set; }

        public Monitor()
        {
            this.helperCode = new HelperCode();
            this._listInitializer = new ListInitializer(this);

            InitializeComponent();
            UpdateList();
        }

        private void Start_Setup(object sender, RoutedEventArgs e)
        {
            // Create a new dictionary for this setup session.
            SetupConfigItems = new Dictionary<string, string>();
            this.setupPage.Visibility = Visibility.Visible;
        }

        private void Exit_Setup(object sender, RoutedEventArgs e)
        {
            this.setupPage.Visibility = Visibility.Collapsed;
        }

        private void URL_Setup(object sender, RoutedEventArgs e)
        {
            this.ChoicePage.Visibility = Visibility.Collapsed;
            this.URL.Visibility = Visibility.Visible;
        }

        private async void Validiate_URL(object sender, RoutedEventArgs e)
        {
            string url = this.URL_Field.Text;
            string cleanedUrl = url.Replace("*", "");

            if (cleanedUrl.StartsWith("http://") || cleanedUrl.StartsWith("https://"))
            {
                try
                {
                    bool isValid = await CheckUrlValidityAsync(cleanedUrl);
                    if (isValid)
                    {
                        // Use the current session's SetupConfigItems dictionary.
                        SetupConfigItems.Add("Method", "urlbased");
                        SetupConfigItems.Add("Code", "0");
                        SetupConfigItems.Add("Count", ExtractNumber(url));
                        SetupConfigItems.Add("url", url);
                        SetupConfigItems.Add("cleaned", cleanedUrl);
                        SetupConfigItems.Add("enabled", "true");

                        this.URL.Visibility = Visibility.Collapsed;
                        this.namePage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.URL_Field.Text = "Invalid URL. Please try again.";
                    }
                }
                catch (Exception)
                {
                    this.URL_Field.Text = "Failed to open the URL. Please try again.";
                }
            }
            else
            {
                this.URL_Field.Text = "Please enter a valid URL starting with http:// or https://.";
            }
        }

        private void NameSetup(object sender, RoutedEventArgs e)
        {
            string name = this.name.Text;
            if (name.Length > 0)
            {
                SetupConfigItems.Add("name", name);
                this.namePage.Visibility = Visibility.Collapsed;
                this.refreshPage.Visibility = Visibility.Visible;
            }
            else
            {
                this.name.Text = "Please enter a valid name.";
            }
        }

        private void RefreshSetup(object sender, RoutedEventArgs e)
        {
            string refresh = refreshrate.Text;
            string selectedItem = (timeUnit.SelectedItem as ComboBoxItem)?.Content as string;
            bool isInteger = int.TryParse(refresh, out int result);

            if (isInteger)
            {
                if (selectedItem != null)
                {
                    SetupConfigItems.Add("refresh", refresh);
                    SetupConfigItems.Add("timeunit", selectedItem);
                    URL_Field.Text = "";
                    name.Text = "";
                    refreshrate.Text = "";
                    timeUnit.SelectedIndex = -1;
                    refreshPage.Visibility = Visibility.Collapsed;
                    successPage.Visibility = Visibility.Visible;
                    // Pass the current session's dictionary to SaveJson.
                    SaveJson(SetupConfigItems);
                }
                else
                {
                    refreshrate.Text = "Please select a time unit.";
                }
            }
            else
            {
                refreshrate.Text = "Please enter a valid number.";
            }
        }

        private void SaveJson(Dictionary<string, string> configItems)
        {
            try
            {
                string path = helperCode.GetJsonLocation();
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

        private async Task<bool> CheckUrlValidityAsync(string cleanedUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(cleanedUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Valid URL.");
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("Invalid URL.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error checking URL: " + ex.Message);
                return false;
            }
        }

        private void UpdateList()
        {
            try
            {
                string path = helperCode.GetJsonLocation();

                if (File.Exists(path))
                {
                    string existingJson = File.ReadAllText(path);

                    var objects = JsonConvert.DeserializeObject<List<MyObject>>(existingJson);

                    _listInitializer.CreateListDescriptionGridAndItems();
                    foreach (var obj in objects)
                    {
                        if (obj.Method == "urlbased")
                        {
                            _listInitializer.URLBased(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating the list: " + ex.Message);
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        public string ExtractNumber(string url)
        {
            string pattern = @"\*(\d+)\*";
            Match match = Regex.Match(url, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            throw new ArgumentException("The URL does not contain a number surrounded by asterisks.");
        }
    }

    public class MyObject
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public string Cleaned { get; set; }
        public string Name { get; set; }
        public string Refresh { get; set; }
        public string TimeUnit { get; set; }
        public string Code { get; set; }
        public string Count { get; set; }
        public string Enabled { get; set; }
    }
}
