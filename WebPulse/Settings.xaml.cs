using System.Windows;
using System.Windows.Controls;

namespace WebPulse
{
    public partial class Settings : UserControl
    {
        public static string Language;
        public static string FontSize;
        
        public static MainWindow Instance { get; private set; }
        
        public Settings(MainWindow mainWindow)
        {
            Instance = mainWindow;
            InitializeComponent();
            
        }


        public void LanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            Language = (this.LanguageComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }

        public void FontSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            FontSize = (this.FontSizeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }



        private void ThemeChanged(object sender, SelectionChangedEventArgs e)
        {
            // Your logic for theme change
            var selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string selectedTheme = selectedItem.Content.ToString();

            if (selectedTheme == "Light")
            {
                DarkMode.Disable();
                Instance.Refresh_Settings();
            }
            else if (selectedTheme == "Dark")
            {
                DarkMode.Enable();
                Instance.Refresh_Settings();
            }
        }

    }
}
