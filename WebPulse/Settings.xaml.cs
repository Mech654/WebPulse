using System.Windows;
using System.Windows.Controls;

namespace WebPulse
{
    public partial class Settings : UserControl
    {
        public static string theme;
        public static string language;
        public static string fontSize;

        public Settings()
        {
            InitializeComponent();
        }


        public void languageChanged(object sender, SelectionChangedEventArgs e)
        {
            language = (this.LanguageComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }

        public void fontSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            fontSize = (this.FontSizeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }



        private void themeChanged(object sender, SelectionChangedEventArgs e)
        {
            // Your logic for theme change
            var selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string selectedTheme = selectedItem.Content.ToString();

            if (selectedTheme == "Light")
            {
                DarkMode.Disable();
            }
            else if (selectedTheme == "Dark")
            {
                DarkMode.Enable();
            }
        }

    }
}
