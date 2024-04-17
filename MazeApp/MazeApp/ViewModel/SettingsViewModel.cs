using MazeApp.Model;
using MazeApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MazeApp.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private Settings settings;
        private Settings originalSettings;
        public SettingsViewModel(Settings settings)
        {
            this.settings = new Settings(settings);
            this.originalSettings = settings;
        }

        public GenerationAlgorithm Algorithm
        {
            get
            {
                return settings.Algorithm;
            }
            set
            {
                settings.Algorithm = value;
                NotifyPropertyChanged(nameof(Algorithm));
                NotifyPropertyChanged(nameof(MazeWidth));
                NotifyPropertyChanged(nameof(MazeHeight));
            }
        }
        public int MazeWidth
        {
            get
            {
                return settings.MazeWidth;
            }
            set
            {
                settings.MazeWidth = value;
                NotifyPropertyChanged(nameof(MazeWidth));
                NotifyPropertyChanged(nameof(MazeHeight));
            }
        }
        public int MazeHeight
        {
            get
            {
                return settings.MazeHeight;
            }
            set
            {
                settings.MazeHeight = value;
                NotifyPropertyChanged(nameof(MazeWidth));
                NotifyPropertyChanged(nameof(MazeHeight));
            }
        }
        public Theme ColourTheme
        {
            get
            {
                return settings.ColourTheme;
            }
            set
            {
                settings.ColourTheme = value;
                ThemeManager.ChangeTheme(settings.ColourTheme);
            }
        }
        public bool IsLimitedVisibility
        {
            get { return settings.IsLimitedVisibility; }
            set
            {
                settings.IsLimitedVisibility = value;
                NotifyPropertyChanged(nameof(IsLimitedVisibility));
            }
        }

        public string PlayerName
        {
            get
            {
                return settings.PlayerName;
            }
            set
            {
                settings.PlayerName = value;
                NotifyPropertyChanged(nameof(PlayerName));
            }
        }
        public IEnumerable<GenerationAlgorithm> GenerationAlgorithmOptions
        {
            get
            {
                return Enum.GetValues(typeof(GenerationAlgorithm)).Cast<GenerationAlgorithm>();
            }
        }

        public IEnumerable<Theme> ColourThemeOptions
        {
            get
            {
                return Enum.GetValues(typeof(Theme)).Cast<Theme>();
            }
        }

        public void ResetTheme()
        {
            ColourTheme = originalSettings.ColourTheme;
        }

        private void SaveSettings()
        {
            this.originalSettings.CopyFrom(this.settings);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand? updater;

        public ICommand UpdateSettingsCommand
        {
            get
            {
                if (updater == null)
                    updater = new SettingsUpdater(this);
                return updater;
            }
            set
            {
                updater = value;
            }
        }

        private class SettingsUpdater : ICommand
        {
            private SettingsViewModel settingsViewModel;
            public SettingsUpdater(SettingsViewModel settingsViewModel)
            {
                this.settingsViewModel = settingsViewModel;
            }
            public bool CanExecute(object? parameter)
            {
                return true;
            }

            public event EventHandler? CanExecuteChanged;

            public void Execute(object? parameter)
            {
                settingsViewModel.SaveSettings();
                if (parameter != null && parameter is Window) ((Window)parameter).Close();
            }
        }
    }
}
