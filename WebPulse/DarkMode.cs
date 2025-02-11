using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WebPulse
{
    internal class DarkMode
    {

        public static void Enable()
        {
            var app = (App)Application.Current;
      
            app.Resources["PrimaryButtonBackgroundBrush"] = new SolidColorBrush((Color)Application.Current.Resources["DarkPrimaryButtonBackgroundColor"]);
            app.Resources["PrimaryButtonTextBrush"] = new SolidColorBrush((Color)Application.Current.Resources["DarkPrimaryButtonTextColor"]);

        }

        public static void Disable()
        {
            var app = (App)Application.Current;
            app.Resources["PrimaryButtonBackgroundBrush"] = new SolidColorBrush((Color)Application.Current.Resources["PrimaryButtonBackgroundColor"]);
            app.Resources["PrimaryButtonTextBrush"] = new SolidColorBrush((Color)Application.Current.Resources["PrimaryButtonTextColor"]);

        }
    }
}
