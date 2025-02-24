using System;
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
        private string ActiveSeeking { get; set; } = " ";
        private string ActiveFound { get; set; } = " ";
        private string ActiveFoundAll { get; set; } = " ";
        private readonly HelperCode _helperCode = new HelperCode();
        public Home()
        {
            InitializeComponent();
            DisplayReleases();
        }

        private void DisplayReleases()
        {
            CreateListDescriptionGridAndItems();
            //I will return here later. Do: 1 . go through every releases in the json file i will make. 2. send it the method
            var objects = _helperCode.GetReleaseJsonObjects();
            foreach (var obj in objects)
            {
                DisplayReleases(obj);
            }
        }

        private void CreateListDescriptionGridAndItems()
        {
            
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            
            
            Brush primaryTextBrush = (Brush)Application.Current.Resources["PrimaryButtonTextBrush"];


            TextBlock nameField = new TextBlock
            {
                Text = "Name",
                FontSize = 14,
                Foreground = primaryTextBrush,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock releasesField = new TextBlock
            {
                Text = "Releases",
                FontSize = 14,
                Foreground = primaryTextBrush,
                FontWeight = FontWeights.Bold,
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
        private void DisplayReleases(MyReleases listObject)
        {
            Brush bgBrush = (Brush)Application.Current.Resources["PrimaryButtonBackgroundBrush"];
            Brush fgBrush = (Brush)Application.Current.Resources["PrimaryButtonTextBrush"];
            
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
           
            grid.Margin = new Thickness(10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock nameField = new TextBlock
            {
                Text = listObject.Name,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = fgBrush,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            TextBlock releasesField = new TextBlock
            {
                Text = listObject.Link,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = fgBrush,
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
}
