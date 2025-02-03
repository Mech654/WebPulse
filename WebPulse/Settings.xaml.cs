using System;
using System.Text;
using System.Linq;
using System.Windows;
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
using System.Diagnostics;

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

        public void themeChanged(object sender, SelectionChangedEventArgs e)
        {
            theme = (this.ThemeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }

        public void languageChanged(object sender, SelectionChangedEventArgs e)
        {
            language = (this.LanguageComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }

        public void fontSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            fontSize = (this.FontSizeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }

        public void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Reloads the listed pages with the new settings
            MainWindow mainWindow = new MainWindow();
            Home home = new Home();
            Monitor monitor = new Monitor();
            Settings settings = new Settings();
        }
    }
}
