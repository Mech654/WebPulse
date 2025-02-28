using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using WebPulse.models;
using WebPulse;

namespace WebPulse
{

    public partial class Home : UserControl
    {

        private readonly HelperCode _helperCode = new HelperCode();
        public Home()
        {
            InitializeComponent();
            DisplayReleases();
        }

        private void DisplayReleases()
        {
            CreateListDescriptionGridAndItems();
            var objects = _helperCode.GetReleaseJsonObjects();

            Dictionary<string, int> groups = new Dictionary<string, int>();

            int y = 0;
            foreach (MyReleases obj in objects)
            {
                y++;
                if (groups.ContainsKey(obj.Name))
                {
                    groups[obj.Name] += 1;
                }
                else
                {
                    groups.Add(obj.Name, 1);
                }
            }
            TotalReleases.Text = y.ToString();

            y = 0;
            foreach (string key in groups.Keys)
            {
                var foundObject = objects.FirstOrDefault(o => o.Name == key);
                DisplayReleases(foundObject, groups[key]);
                y++;
            }
            Releases.Text = y.ToString();

            var objects2 = _helperCode.GetSetupJsonObjects();

            y = 0;
            foreach (var obj in objects2)
            {
                y++;
            }
            
            Monitoring.Text = y.ToString();


        }

        private void CreateListDescriptionGridAndItems()
        {
            
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) });
            
            
            Brush primaryTextBrush = (Brush)Application.Current.Resources["PrimaryButtonTextBrush"];


            TextBlock nameField = new TextBlock
            {
                Text = "Name",
                FontSize = 12,
                Foreground = primaryTextBrush,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock releasesField = new TextBlock
            {
                Text = "First release",
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                Foreground = primaryTextBrush,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            
            TextBlock count = new TextBlock
            {
                Text = "Total Releases",
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12,
                Foreground = primaryTextBrush,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            
            TextBlock number = new TextBlock
            {
                Text = "Next",
                FontSize = 12,
                Foreground = primaryTextBrush,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            
            Grid.SetColumn(nameField, 0);
            Grid.SetColumn(count, 1);
            Grid.SetColumn(number, 3);
            Grid.SetColumn(releasesField, 2);

            grid.Children.Add(nameField);
            grid.Children.Add(releasesField);
            grid.Children.Add(count);
            grid.Children.Add(number);
            
            ReleaseList.Children.Add(grid);
        }
        private void DisplayReleases(MyReleases listObject, int number)
        {
            Brush bgBrush = (Brush)Application.Current.Resources["PrimaryButtonBackgroundBrush"];
            Brush fgBrush = (Brush)Application.Current.Resources["PrimaryButtonTextBrush"];
            
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) });

            TextBlock nameField = new TextBlock
            {
                Text = listObject.Name,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                Foreground = fgBrush,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock releasesField = new TextBlock
            {
                Text = listObject.Title,
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                Foreground = fgBrush,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
            releasesField.PreviewMouseLeftButtonDown  += (s, e) => Process.Start(new ProcessStartInfo(listObject.Link) { UseShellExecute = true });
            

            TextBlock count = new TextBlock
            {
                Text = number.ToString(),
                FontSize = 10,
                FontWeight = FontWeights.Bold,

                Foreground = fgBrush,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
            
            TextBlock episode = new TextBlock
            {
                Text = listObject.Number,
                FontSize = 10,
                FontWeight = FontWeights.Bold,

                Foreground = fgBrush,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };

            Grid.SetColumn(nameField, 0);
            Grid.SetColumn(count, 1);
            Grid.SetColumn(episode, 2);
            Grid.SetColumn(releasesField, 3);

            grid.Children.Add(nameField);
            grid.Children.Add(releasesField);
            grid.Children.Add(count);
            grid.Children.Add(episode);

            ReleaseList.Children.Add(grid);
        }
    }
}
