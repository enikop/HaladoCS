using MazeApp.Model;
using MazeApp.ViewModel;
using System.Windows;

namespace MazeApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MenuViewModel menuViewModel;
        private readonly Settings settings;

        public MainWindow()
        {
            InitializeComponent();
            settings = new Settings();
            menuViewModel = new MenuViewModel(settings);
            DataContext = this.menuViewModel;
        }
    }
}
