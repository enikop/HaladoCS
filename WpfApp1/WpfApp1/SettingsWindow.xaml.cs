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

namespace WpfApp1
{
    public class SettingsViewModel
    {
        public Settings Settings { get; set; }
        public List<Brush> Brushes { get; set; }

        public SettingsViewModel(Settings settings, List<Brush> brushes)
        {
            Settings = settings;
            Brushes = brushes;
        }
    }
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly Settings oldSettings;
        private readonly Settings newSettings;

        public SettingsWindow(Settings settings)
        {
            InitializeComponent();
            algorithmComboBox.ItemsSource = Enum.GetValues(typeof(GenerationAlgorithm)).Cast<GenerationAlgorithm>();
            colourThemeComboBox.ItemsSource = Enum.GetValues(typeof(ColourTheme)).Cast<ColourTheme>();

            this.newSettings = new Settings(settings);
            this.oldSettings = settings;

            DataContext = this.newSettings;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            oldSettings.CopyFrom(newSettings);  
            Close();
        }
    }
}
