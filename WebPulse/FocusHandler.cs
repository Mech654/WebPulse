using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WebPulse
{
    internal class FocusHandler
    {
        private HelperCode _helperCode = new HelperCode();
        public void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string[] tagValues = (string[])textBox.Tag;


            string _name = tagValues[0];
            string _type = tagValues[1];
            string updatedValue = textBox.Text;

            UpdateConfig(_name, _type, updatedValue);
        }

        private void UpdateConfig(string name, string type, string value)
        {
            _helperCode.UpdateJsonValue(name, type, value);
        }
    }
}
