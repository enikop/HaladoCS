using MazeApp.Model;
using MazeApp.ViewModel;
using System;
using System.Windows;

namespace MazeApp.View
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
