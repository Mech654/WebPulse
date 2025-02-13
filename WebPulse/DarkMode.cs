using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WebPulse
{
    internal static class DarkMode
    {

        public static void Enable()
        {
            var app = (App)Application.Current;
      
            app.Resources["PrimaryButtonBackgroundBrush"] = new SolidColorBrush((Color)Application.Current.Resources["DarkPrimaryButtonBackgroundColor"]);
            app.Resources["PrimaryButtonTextBrush"] = new SolidColorBrush((Color)Application.Current.Resources["DarkPrimaryButtonTextColor"]);
            app.Resources["TextBlockBrush"] = new SolidColorBrush((Color)Application.Current.Resources["DarkTextBlockColor"]);
            app.Resources["ControlBackgroundBrush"] = new SolidColorBrush((Color)Application.Current.Resources["DarkControlBackgroundColor"]);
            app.Resources["MainContentBackground"] = new SolidColorBrush((Color)Application.Current.Resources["DarkMainContentBackgroundColor"]);
            app.Resources["BorderLineBrush"] = new SolidColorBrush((Color)Application.Current.Resources["DarkBorderLineColor"]);

        }

        public static void Disable()
        {
            var app = (App)Application.Current;
            app.Resources["PrimaryButtonBackgroundBrush"] = new SolidColorBrush((Color)Application.Current.Resources["PrimaryButtonBackgroundColor"]);
            app.Resources["PrimaryButtonTextBrush"] = new SolidColorBrush((Color)Application.Current.Resources["PrimaryButtonTextColor"]);
            app.Resources["TextBlockBrush"] = new SolidColorBrush((Color)Application.Current.Resources["TextBlockColor"]);
            app.Resources["ControlBackgroundBrush"] = new SolidColorBrush((Color)Application.Current.Resources["ControlBackgroundColor"]);
            app.Resources["MainContentBackground"] = new SolidColorBrush((Color)Application.Current.Resources["MainContentBackgroundColor"]);
            app.Resources["BorderLineBrush"] = new SolidColorBrush((Color)Application.Current.Resources["BorderLineColor"]);
        }
    }
}
