using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WebPulse.models;

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
            Brush fgBrush = (Brush)Application.Current.Resources["PrimaryButtonTextBrush"];
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(10, 10, 10, 10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock nameField = new TextBlock
            {
                Text = "Name",
                FontSize = 12,
                Background = Brushes.Transparent,
                Foreground = fgBrush,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock urlField = new TextBlock
            {
                Text = "URL/Code",
                FontSize = 12,
                Background = Brushes.Transparent,
                Foreground = fgBrush,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock timeField = new TextBlock
            {
                Text = "Refresh ",
                FontSize = 12,
                Background = Brushes.Transparent,
                Foreground = fgBrush,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Right,
                TextAlignment = TextAlignment.Center
            };
            
            TextBlock timeField2 = new TextBlock
            {
                Text = "  Rate",
                FontSize = 12,
                Background = Brushes.Transparent,
                Foreground = fgBrush,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Center
            };



            TextBlock remainingField = new TextBlock
            {
                Text = "Releases",
                FontSize = 12,
                Background = Brushes.Transparent,
                Foreground = fgBrush,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock buttonField = new TextBlock
            {
                Text = "",
                FontSize = 12,
                Background = Brushes.Transparent,
                Foreground = fgBrush,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            Grid.SetColumn(nameField, 0);
            Grid.SetColumn(urlField, 1);
            Grid.SetColumn(timeField, 2);
            Grid.SetColumn(timeField2, 3);
            Grid.SetColumn(remainingField, 4);
            Grid.SetColumn(buttonField, 5);

            grid.Children.Add(nameField);
            grid.Children.Add(urlField);
            grid.Children.Add(timeField);
            grid.Children.Add(timeField2);

            grid.Children.Add(remainingField);
            grid.Children.Add(buttonField);

            _monitor.DynamicListBox.Children.Add(grid);
        }

        public void UrlBased(MyObject listObject)
        {
            Brush bgBrush = (Brush)Application.Current.Resources["PrimaryButtonBackgroundBrush"];
            Brush fgBrush = (Brush)Application.Current.Resources["PrimaryButtonTextBrush"];
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Background = bgBrush;
            grid.Margin = new Thickness(10, 10, 10, 10);

            
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            
            TextBox name = new TextBox
            {
                Text = listObject.Name,
                BorderThickness = new Thickness(0),
                Height = 40,
                Foreground = fgBrush,
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
                Foreground = fgBrush,
                FontSize = 12,
                Background = Brushes.Transparent,
                Padding = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            TextBox time = new TextBox
            {
                Text = listObject.Refresh,
                BorderThickness = new Thickness(0),
                Height = 40,
                Foreground = fgBrush,
                FontSize = 12,
                Background = Brushes.Transparent,
                Padding = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };

            ComboBox unit = new ComboBox
            {
                SelectedItem = listObject.TimeUnit,
                BorderThickness = new Thickness(0),
                Foreground = fgBrush,
                Background = bgBrush,
                Height = 40,
                FontSize = 12, 
                HorizontalContentAlignment = HorizontalAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Top
            };
            
            unit.Style = (Style)_monitor.FindResource(typeof(ComboBox));


            TextBlock releases = new TextBlock
            {
                Text = "3",
             
                Height = 40,
                Foreground = fgBrush,
                FontSize = 12,
                Background = Brushes.Transparent,
                Padding = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            CheckBox lightCheckBox = new CheckBox
            {
                IsChecked = true,
                ToolTip = "Enable/Disable",
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
            Grid.SetColumn(releases, 4);
            Grid.SetColumn(lightCheckBox, 5);

            // Always put the listobject name as first item and the type of item as second item in the tag
            name.Tag = new [] { listObject.Name, "name" };
            url.Tag = new [] { listObject.Name, "url" };
            time.Tag = new [] { listObject.Name, "refresh" };
            unit.Tag = new [] { listObject.Name, "TimeUnit" };
            releases.Tag = new [] { listObject.Name, "remaining" };
            lightCheckBox.Tag = new [] { listObject.Name, "light" };

            FocusHandler focusHandler = new FocusHandler();

            // Eventhandlers
            url.LostFocus += focusHandler.LostFocus;
            name.LostFocus += focusHandler.LostFocus;
            time.LostFocus += focusHandler.LostFocus;
            unit.SelectionChanged += focusHandler.LostFocus;
            releases.LostFocus += focusHandler.LostFocus;
            lightCheckBox.LostFocus += focusHandler.LostFocus;


            grid.Children.Add(name);
            grid.Children.Add(url);
            grid.Children.Add(time);
            grid.Children.Add(unit);
            grid.Children.Add(releases);
            grid.Children.Add(lightCheckBox);

            _monitor.DynamicListBox.Children.Add(grid);


        }
    }
}
