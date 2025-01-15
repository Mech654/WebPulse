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

namespace WebPulse
{
    /// <summary>
    /// Interaction logic for Monitor.xaml
    /// </summary>
    public partial class Monitor : UserControl
    {
        Dictionary<string, string> setupConfigItems = new Dictionary<string, string>();
        public Monitor()
        {
            InitializeComponent();
        }
        //remember, always make earlier page visibility collapsed
        private void Exit_Setup(object sender, RoutedEventArgs e)
        {
            this.setupPage.Visibility = Visibility.Collapsed;
        }

        private void URL_Setup(object sender, RoutedEventArgs e)
        {
            this.ChoicePage.Visibility = Visibility.Collapsed;
            this.URL.Visibility = Visibility.Visible;
        }

        private void Validiate_URL(object sender, RoutedEventArgs e)
        {
            // Get the URL from the TextBox
            string url = this.URL_Field.Text;
            string cleanedUrl = url.Replace("*", ""); // Remove '*' characters

            // Check if the cleaned URL starts with "http://" or "https://"
            if (cleanedUrl.StartsWith("http://") || cleanedUrl.StartsWith("https://"))
            {
                try
                {
                    // Attempt to open the URL in the default browser
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = cleanedUrl,
                        UseShellExecute = true // Ensures the URL opens in the default browser
                    });
                    setupConfigItems.Add("Method", "urlbased");
                    setupConfigItems.Add("url", url);
                    setupConfigItems.Add("cleaned", cleanedUrl);
                    this.URL.Visibility = Visibility.Collapsed;
                    this.namePage.Visibility = Visibility.Visible;


                }
                catch (Exception ex)
                {
                    // If there is an error opening the URL, set the TextBox to an error message
                    this.URL_Field.Text = "Failed to open the URL. Please try again.";
                }
                
            }
            else
            {
                // If the URL doesn't start with "http://" or "https://", show a message
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
                    this.setupConfigItems.Clear();
                    this.refreshPage.Visibility = Visibility.Collapsed;
                    this.successPage.Visibility = Visibility.Visible;
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
    }
}
