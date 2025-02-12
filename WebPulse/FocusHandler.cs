using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            string updatedValue = string.Empty;
            string _name = "";
            string _type = "";

            switch (sender)
            {
                case TextBox textBox:
                {
                    updatedValue = textBox.Text;
                    string[] tagValues = (string[])textBox.Tag;
                    _name = tagValues[0];
                    _type = tagValues[1];
                    break;
                }
                case ComboBox comboBox:
                {
                    updatedValue = comboBox.SelectedItem.ToString();
                    string[] tagValues = (string[])comboBox.Tag;
                    _name = tagValues[0];
                    _type = tagValues[1];
                    break;
                }
                default:
                    return;
            }

            Debug.WriteLine("Updating config: " + _name + " type: " + _type + " new value: " + updatedValue);

            UpdateConfig(_name, _type, updatedValue);
        }


        private void UpdateConfig(string name, string type, string value)
        {
            _helperCode.UpdateJsonValue(name, type, value);
        }
    }
}
