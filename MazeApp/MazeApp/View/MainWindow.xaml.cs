using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MazeApp.ViewModel;
using MazeApp.Model;

namespace MazeApp.Helpers
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
