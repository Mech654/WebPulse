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
using WebPulse.models;

namespace WebPulse
{
    public partial class Monitor 
    {
        private readonly HelperCode _helperCode;
        private readonly ListInitializer _listInitializer;
        private Dictionary<string, string> SetupConfigItems { get; set; }

        public Monitor()
        {
            this._helperCode = new HelperCode();
            this._listInitializer = new ListInitializer(this);

            InitializeComponent();
            UpdateList();
        }

        #region Setup

         private void Start_Setup(object sender, RoutedEventArgs e)
        {
            // Create a new dictionary for this setup session.
            SetupConfigItems = new Dictionary<string, string>();
            this.SetupPage.Visibility = Visibility.Visible;
        }

        private void Exit_Setup(object sender, RoutedEventArgs e)
        {
            this.SetupPage.Visibility = Visibility.Collapsed;
        }

        private void URL_Setup(object sender, RoutedEventArgs e)
        {
            this.ChoicePage.Visibility = Visibility.Collapsed;
            this.Url.Visibility = Visibility.Visible;
        }

        private async void Validiate_URL(object sender, RoutedEventArgs e)
        {
            string url = this.UrlField.Text;
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

                        this.Url.Visibility = Visibility.Collapsed;
                        this.NamePage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.UrlField.Text = "Invalid URL. Please try again.";
                    }
                }
                catch (Exception)
                {
                    this.UrlField.Text = "Failed to open the URL. Please try again.";
                }
            }
            else
            {
                this.UrlField.Text = "Please enter a valid URL starting with http:// or https://.";
            }
        }

        private void NameSetup(object sender, RoutedEventArgs e)
        {
            string name = this.Name.Text;
            if (name.Length > 0)
            {
                SetupConfigItems.Add("name", name);
                this.NamePage.Visibility = Visibility.Collapsed;
                this.RefreshPage.Visibility = Visibility.Visible;
            }
            else
            {
                this.Name.Text = "Please enter a valid name.";
            }
        }

        private void RefreshSetup(object sender, RoutedEventArgs e)
        {
            string refresh = Refreshrate.Text;
            string selectedItem = (TimeUnit.SelectedItem as ComboBoxItem)?.Content as string;
            bool isInteger = int.TryParse(refresh, out _);

            if (isInteger)
            {
                if (selectedItem != null)
                {
                    SetupConfigItems.Add("refresh", refresh);
                    SetupConfigItems.Add("timeunit", selectedItem);
                    UrlField.Text = "";
                    Name.Text = "";
                    Refreshrate.Text = "";
                    TimeUnit.SelectedIndex = -1;
                    RefreshPage.Visibility = Visibility.Collapsed;
                    SuccessPage.Visibility = Visibility.Visible;
                    // Pass the current session's dictionary to SaveJson.
                    SaveJson(SetupConfigItems);
                }
                else
                {
                    Refreshrate.Text = "Please select a time unit.";
                }
            }
            else
            {
                Refreshrate.Text = "Please enter a valid number.";
            }
        }

        #endregion
        
       

        private void SaveJson(Dictionary<string, string> configItems)
        {
            try
            {
                string path = _helperCode.GetSetupJson();
                Debug.WriteLine("File path: " + path);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    if (directory != null) Directory.CreateDirectory(directory);
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
                using HttpClient client = new HttpClient();
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
                string path = _helperCode.GetSetupJson();

                if (File.Exists(path))
                {
                    string existingJson = File.ReadAllText(path);

                    var objects = JsonConvert.DeserializeObject<List<MyObject>>(existingJson);

                    _listInitializer.CreateListDescriptionGridAndItems();
                    foreach (var obj in objects)
                    {
                        if (obj.Method == "urlbased")
                        {
                            _listInitializer.UrlBased(obj);
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

        private string ExtractNumber(string url)
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
}
