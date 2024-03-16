using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MazeApp.Model;
using MazeApp.View;

namespace MazeApp.ViewModel
{
    public class MenuViewModel: INotifyPropertyChanged
    {
        private readonly Settings settings;

        public AppTheme CurrentTheme
        {
            get
            {
               return settings.CurrentTheme;
            }
        }
        public ICommand OpenMultiplayerCommand { get; set; }
        public ICommand OpenSingleplayerCommand { get; set; }
        public ICommand OpenSettingsCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public MenuViewModel(Settings settings) {
            this.settings = settings;
            OpenMultiplayerCommand = new OpenWindowCommand(typeof(MultiplayerWindow), settings);
            OpenSettingsCommand = new OpenWindowCommand(typeof(SettingsWindow), settings);
            OpenSingleplayerCommand = new OpenWindowCommand(typeof(SingleplayerWindow), settings);
            CloseCommand = new CloseWindowCommand();

            //Subscribe to property changes in settings
            this.settings.PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(settings.CurrentTheme))
            {
                // Notify that CurrentTheme property has changed
                NotifyPropertyChanged(nameof(CurrentTheme));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        
    }
}
