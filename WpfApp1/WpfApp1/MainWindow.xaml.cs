using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Settings settings;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            List<AppTheme> themes = new();
            themes.Add(new AppTheme(ColourTheme.Dark, Colors.Black, Colors.AntiqueWhite, Colors.MediumAquamarine, Colors.Orchid, Colors.Aquamarine, Colors.DarkOrchid));
            themes.Add(new AppTheme(ColourTheme.Light, Colors.White, Colors.Black, Colors.Aquamarine, Colors.Orchid, Colors.Aquamarine , Colors.DarkOrchid));
            themes.Add(new AppTheme(ColourTheme.Pink, Color.FromRgb(255, 155, 210), Color.FromRgb(64, 43, 58), Color.FromRgb(74, 129, 74), Color.FromRgb(248, 244, 236), Color.FromRgb(214, 52, 132), Color.FromRgb(74, 129, 74)));
            settings = new Settings(themes);
            DataContext = this.settings;
        }

        private void Singleplayer_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new SingleplayerWindow());
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Multiplayer_Click(object sender, RoutedEventArgs e)
        {
            MultiplayerWindow multiplayerWindow = new(settings);
            multiplayerWindow.Show();
            this.Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new(settings);
            settingsWindow.Show();
        }
        /*private void DrawMaze()
        {
            List<(Point, Point)> walls = maze.GetEdgeCoordinates(cellSize);

            Rectangle rectangle = new Rectangle
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Width = maze.Width * cellSize,
                Height = maze.Height * cellSize
            };
            mazeCanvas.Children.Add(rectangle);

            foreach (var wall in walls)
            {
                Line line = new Line
                {
                    X1 = wall.Item1.X,
                    Y1 = wall.Item1.Y,
                    X2 = wall.Item2.X,
                    Y2 = wall.Item2.Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                mazeCanvas.Children.Add(line);
            }
        }*/
    }
}
