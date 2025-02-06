using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WebPulse
{

    public partial class Home : UserControl
    {

        public Home()
        {
            InitializeComponent();
            DisplayReleases();
        }

        private void DisplayReleases()
        {
            CreateListDescriptionGridAndItems();
            //I will return here later. Do: 1 . go through every releases in the json file i will make. 2. send it the method
        }

        private void CreateListDescriptionGridAndItems()
        {
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock nameField = new TextBlock
            {
                Text = "Name",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock releasesField = new TextBlock
            {
                Text = "Releases",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            Grid.SetColumn(nameField, 0);
            Grid.SetColumn(releasesField, 1);

            grid.Children.Add(nameField);
            grid.Children.Add(releasesField);

            ReleaseList.Children.Add(grid);
        }
        private void URLBased(MyObject listObject)
        {
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"));
            grid.Margin = new Thickness(10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            TextBlock nameField = new TextBlock
            {
                Text = listObject.Name,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock releasesField = new TextBlock
            {
                Text = listObject.Url,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            Grid.SetColumn(nameField, 0);
            Grid.SetColumn(releasesField, 1);

            grid.Children.Add(nameField);
            grid.Children.Add(releasesField);

            ReleaseList.Children.Add(grid);
        }
    }

    public class Releases
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
