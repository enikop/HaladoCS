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
using MazeApp.ViewModel;
using MazeApp.Model;

namespace MazeApp.Helpers
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        private readonly SettingsViewModel settingsViewModel;

        public SettingsWindow(Settings settings)
        {
            InitializeComponent();
            settingsViewModel = new SettingsViewModel(settings);

            DataContext = this.settingsViewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            settingsViewModel.ResetTheme();
            base.OnClosed(e);

        }
    }
}
