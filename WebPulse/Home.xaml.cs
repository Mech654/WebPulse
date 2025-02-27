using System;
using System.Diagnostics;
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

            int x = 1;
            string y = "0";
            foreach (var obj in objects)
            {
                Debug.WriteLine("Atleast this will work right?");
                if (y == "0")
                {
                    Debug.WriteLine("FIRST TIME");
                    y = obj.Name;
                    x++;
                }
                else if (obj.Name == y)
                {
                    x++;
                    Debug.WriteLine("SECOND TIME");
                }
                else
                {
                    Debug.WriteLine("THIRD TIME");
                    y = obj.Name;
                    DisplayReleases(obj, x);
                    x = 1;
                }
            }
        }

        private void CreateListDescriptionGridAndItems()
        {
            
            Grid grid = new Grid();
            grid.Height = Double.NaN;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(10);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
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
            
            TextBlock count = new TextBlock
            {
                Text = "Total Releases",
                FontSize = 14,
                Foreground = primaryTextBrush,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            


            Grid.SetColumn(nameField, 0);
            Grid.SetColumn(releasesField, 2);
            Grid.SetColumn(count, 1);

            grid.Children.Add(nameField);
            grid.Children.Add(releasesField);
            grid.Children.Add(count);
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
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock nameField = new TextBlock
            {
                Text = listObject.Name,
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

            Grid.SetColumn(nameField, 0);
            Grid.SetColumn(releasesField, 2);
            Grid.SetColumn(count, 1);

            grid.Children.Add(nameField);
            grid.Children.Add(releasesField);
            grid.Children.Add(count);

            ReleaseList.Children.Add(grid);
        }
    }
}
