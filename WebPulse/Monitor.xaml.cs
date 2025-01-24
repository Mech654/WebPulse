using System;
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
using System.Windows.Controls.Primitives;
using System.Management.Instrumentation;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Media;

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
        private void Start_Setup(object sender, RoutedEventArgs e)
        {
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


                    CreateListDescriptionGridAndItems();
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




        private void CreateListDescriptionGridAndItems()
        {
           
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(10,10,10,10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });




            TextBox nameField = new TextBox
            {
                Text = "Name",
                FontSize = 12,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBox urlField = new TextBox
            {
                Text = "URL/Code",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                TextAlignment = TextAlignment.Center
            };

            TextBox timeField = new TextBox
            {
                Text = "Refresh",
                FontSize = 12,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBox unitField = new TextBox
            {
                Text = "Time Unit",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                TextAlignment = TextAlignment.Center
            };

            TextBox remainingField = new TextBox
            {
                Text = "Remaining",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                TextAlignment = TextAlignment.Center
            };

            TextBox buttonField = new TextBox
            {
                Text = "",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                TextAlignment = TextAlignment.Center
            };


            Grid.SetColumn(nameField, 0);
            Grid.SetColumn(urlField, 1);
            Grid.SetColumn(timeField, 2);
            Grid.SetColumn(unitField, 3);
            Grid.SetColumn(remainingField, 4);
            Grid.SetColumn(buttonField, 5);

            grid.Children.Add(nameField);
            grid.Children.Add(urlField);
            grid.Children.Add(timeField);
            grid.Children.Add(unitField);
            grid.Children.Add(remainingField);
            grid.Children.Add(buttonField);


            DynamicListBox.Children.Add(grid);
        }


        private void URLBased(MyObject listObject)
        {
           

            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"));
            grid.Margin = new Thickness(10, 10, 10, 10);


            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();
            ColumnDefinition col3 = new ColumnDefinition();
            ColumnDefinition col4 = new ColumnDefinition();
            ColumnDefinition col5 = new ColumnDefinition();

            ColumnDefinition col6 = new ColumnDefinition();
            ColumnDefinition col7 = new ColumnDefinition();
            ColumnDefinition col8 = new ColumnDefinition();

            col1.Width = new GridLength(0.8, GridUnitType.Star);
            col2.Width = new GridLength(1, GridUnitType.Star);
            col3.Width = new GridLength(0.5, GridUnitType.Star);
            col4.Width = new GridLength(70, GridUnitType.Pixel);
            col5.Width = new GridLength(1.2, GridUnitType.Star);

            col6.Width = new GridLength(1, GridUnitType.Star);
     
            

            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);
            grid.ColumnDefinitions.Add(col3);
            grid.ColumnDefinitions.Add(col4);
            grid.ColumnDefinitions.Add(col5);
            grid.ColumnDefinitions.Add(col6);


            TextBox name = new TextBox
            {
                Text = listObject.Name,
                BorderThickness = new Thickness(0),
                Height = 40,
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 12,
                TextAlignment = TextAlignment.Center,
                Padding = new Thickness(2),
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                MaxLines = 2,
                TextDecorations = TextDecorations.Underline,
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center

            };

            TextBox url = new TextBox
            {
                Text = listObject.Url,
                BorderThickness = new Thickness(0),
                Height = 40,
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 12,
                Background = Brushes.Transparent,
                Padding = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            TextBox time = new TextBox
            {
                Text = listObject.Refresh.ToString(),
                BorderThickness = new Thickness(0),
                Height = 40,
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 12,
                Background = Brushes.Transparent,
                Padding = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            ComboBox unit = new ComboBox
            {
                SelectedItem = listObject.TimeUnit,
                BorderThickness = new Thickness(0),
                Height = 40,
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 12,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Top
            };

            TextBox remaining = new TextBox
            {
                Text = "1 hour 30 minutes",
                BorderThickness = new Thickness(0),
                Height = 40,
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 12,
                Background = Brushes.Transparent,
                Padding = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };





            unit.Items.Add(listObject.TimeUnit);
            unit.Items.Add("Minutes");
            unit.Items.Add("Hours");
            unit.Items.Add("Days");
            unit.Items.Add("Weeks");


            Grid.SetColumn(name, 0);
            Grid.SetColumn(url, 1);
            Grid.SetColumn(time, 2);
            Grid.SetColumn(unit, 3);
            Grid.SetColumn(remaining, 4);

            grid.Children.Add(name);
            grid.Children.Add(url);
            grid.Children.Add(time);
            grid.Children.Add(unit);
            grid.Children.Add(remaining);


            DynamicListBox.Children.Add(grid);

            unit.Loaded += (s, e) =>
            {
                var popupContainer = (Popup)unit.Template.FindName("PART_Popup", unit);
                if (popupContainer != null)
                {
                    popupContainer.PlacementTarget = unit;
                    popupContainer.Placement = PlacementMode.Right;
                }
            };
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
