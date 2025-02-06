using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WebPulse
{
    public class ListInitializer
    {
        private Monitor _monitor;

        public ListInitializer(Monitor monitor)
        {
            _monitor = monitor;
        }

        public void CreateListDescriptionGridAndItems()
        {
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(10, 10, 10, 10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock nameField = new TextBlock
            {
                Text = "Name",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock urlField = new TextBlock
            {
                Text = "URL/Code",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock timeField = new TextBlock
            {
                Text = "Refresh",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock unitField = new TextBlock
            {
                Text = "Time Unit",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock remainingField = new TextBlock
            {
                Text = "Remaining",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock buttonField = new TextBlock
            {
                Text = "",
                FontSize = 12,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
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

            _monitor.DynamicListBox.Children.Add(grid);
        }

        public void URLBased(MyObject listObject)
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
                HorizontalAlignment = HorizontalAlignment.Center,
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

            CheckBox lightCheckBox = new CheckBox
            {
                Content = "Enable",
                IsChecked = true,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
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
            Grid.SetColumn(lightCheckBox, 5);

            // Always put the listobject name as first item and the type of item as second item in the tag
            name.Tag = new String[] { listObject.Name, "name" };
            url.Tag = new String[] { listObject.Name, "url" };
            time.Tag = new String[] { listObject.Name, "time" };
            unit.Tag = new String[] { listObject.Name, "unit" };
            remaining.Tag = new String[] { listObject.Name, "remaining" };
            lightCheckBox.Tag = new String[] { listObject.Name, "light" };

            // Eventhandlers
            url.LostFocus += LostFocus;
            name.LostFocus += LostFocus;
            time.LostFocus += LostFocus;
            unit.LostFocus += LostFocus;
            remaining.LostFocus += LostFocus;
            lightCheckBox.LostFocus += LostFocus;






            grid.Children.Add(name);
            grid.Children.Add(url);
            grid.Children.Add(time);
            grid.Children.Add(unit);
            grid.Children.Add(remaining);
            grid.Children.Add(lightCheckBox);

            _monitor.DynamicListBox.Children.Add(grid);

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
        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string[] tagValues = (string[])textBox.Tag;

            string setitem1 = tagValues[0];
            string setitem2 = tagValues[1];
            string updatedValue = textBox.Text;

        }

    }
}
