using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MazeApp.Model;
using MazeApp.View;

namespace MazeApp.ViewModel
{
    public class MenuViewModel: INotifyPropertyChanged
    {
        private readonly Settings settings;
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
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        
    }
}
