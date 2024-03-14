using MazeApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MazeApp.ViewModel
{ 
    public class MultiplayerViewModel: INotifyPropertyChanged
    {
        private readonly Settings settings;

        public GenerationAlgorithm Algorithm
        {
            get
            {
                return settings.Algorithm;
            }
        }

        public int MazeWidth
        {
            get
            {
                return settings.MazeWidth;
            }
        }

        public int MazeHeight
        {
            get
            {
                return settings.MazeHeight;
            }
        }

        public AppTheme CurrentTheme
        {
            get
            {
                return settings.CurrentTheme;
            }
        }

        public MultiplayerViewModel(Settings settings)
        {
            this.settings = settings;
       
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
