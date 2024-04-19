using MazeApp.Model;
using MazeApp.ViewModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MazeApp.View
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public CloseWindowCommand CloseCommand { get; }

        public HelpWindow()
        {
            CloseCommand = new CloseWindowCommand();
            InitializeComponent();
            DataContext = this;
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
