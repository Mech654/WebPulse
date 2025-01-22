﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Net.Http;

namespace WebPulse
{
    public partial class Monitor : UserControl
    {
        Dictionary<string, string> setupConfigItems = new Dictionary<string, string>();

        public Monitor()
        {
            InitializeComponent();
            UpdateList();
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
                        setupConfigItems.Add("Method", "urlbased");
                        setupConfigItems.Add("url", url);
                        setupConfigItems.Add("cleaned", cleanedUrl);
                        this.URL.Visibility = Visibility.Collapsed;
                        this.namePage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.URL_Field.Text = "Invalid URL. Please try again.";
                    }
                }
                catch (Exception ex)
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
                setupConfigItems.Add("name", name);
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
            string refresh = this.refreshrate.Text;
            string selectedItem = (timeUnit.SelectedItem as ComboBoxItem)?.Content as string;
            bool isInteger = int.TryParse(refresh, out int result);
            if (isInteger)
            {
                if (selectedItem != null)
                {
                    setupConfigItems.Add("refresh", refresh);
                    setupConfigItems.Add("timeunit", selectedItem);
                    this.URL_Field.Text = "";
                    this.name.Text = "";
                    this.refreshrate.Text = "";
                    this.timeUnit.SelectedIndex = -1;
                    this.refreshPage.Visibility = Visibility.Collapsed;
                    this.successPage.Visibility = Visibility.Visible;
                    SaveJson();
                }
                else
                {
                    this.refreshrate.Text = "Please select a time unit.";
                }
            }
            else
            {
                this.refreshrate.Text = "Please enter a valid number.";
            }
        }

        private void SaveJson()
        {
            try
            {
                string path = "C:\\Users\\ahme1636\\OneDrive - Syddansk Erhvervsskole\\Dokumenter\\Webpulse\\WebPulse\\jsonn\\SetupJson.json";
                Debug.WriteLine("File path: " + path);

                string directory = System.IO.Path.GetDirectoryName(path);
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

                jsonList.Add(new Dictionary<string, string>(setupConfigItems));

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
                string path = "C:\\Users\\ahme1636\\OneDrive - Syddansk Erhvervsskole\\Dokumenter\\Webpulse\\WebPulse\\jsonn\\SetupJson.json";

                if (File.Exists(path))
                {

                    string existingJson = File.ReadAllText(path);


                    var objects = JsonConvert.DeserializeObject<List<MyObject>>(existingJson);


                    foreach (var obj in objects)
                    {
                        if (obj.Method == "urlbased")
                        {
                            URLBased(obj);
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






        private void URLBased(MyObject listObject)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Style = (Style)this.Resources["CustomStackPanelStyle"];


            //make it so the grid's height is 100% of stackpanel
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Colors.LightGray);
            grid.Height = 100;
            grid.VerticalAlignment = VerticalAlignment.Stretch;

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col1);

            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(2, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col2);


            ColumnDefinition col3 = new ColumnDefinition();
            col3.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col3);

            ColumnDefinition col4 = new ColumnDefinition();
            col4.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col4);

            TextBlock textBlock = new TextBlock
            {
                Text = "AAAAAAAAAAAAAAAAAAAAA",
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 16,
                Background = new SolidColorBrush(Colors.Red),
                Padding = new Thickness(10)
            };




            Grid.SetColumn(textBlock, 0);
            grid.Children.Add(textBlock);

            stackPanel.Children.Add(grid);
            DynamicListBox.Children.Add(stackPanel);
        }
    }

    public class MyObject
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public string Cleaned { get; set; }
        public string Name { get; set; }
        public int Refresh { get; set; }
        public string TimeUnit { get; set; }
    }
}
