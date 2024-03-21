using MazeApp.Model;
using MazeApp.Model.Enums;
using MazeApp.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MazeApp.View
{
    /// <summary>
    /// Interaction logic for SingleplayerWindow.xaml
    /// </summary>
    public partial class SingleplayerWindow : Window
    {
        private readonly SingleplayerViewModel singleplayerViewModel;
        public SingleplayerWindow(Settings settings)
        {
            this.singleplayerViewModel = new SingleplayerViewModel(settings);
            DataContext = this.singleplayerViewModel;
            this.PreviewKeyDown += Multiplayer_PreviewKeyDown;
            this.PreviewKeyUp += Multiplayer_PreviewKeyUp;
            InitializeComponent();
            DrawGame();

        }

        private void Multiplayer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UpdatePlayerKeyState(e.Key, true);
        }

        private void Multiplayer_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            UpdatePlayerKeyState(e.Key, false);
        }

        public void UpdatePlayerKeyState(Key key, bool pressed)
        {
            Direction playerDir = singleplayerViewModel.PlayerOneMoveDirection;
            // Update player 1 key states
            if (key == Key.Up || key == Key.W)
                singleplayerViewModel.PlayerOneMoveDirection = GameAssistant.GetUpdatedDirection(playerDir, Direction.North, pressed);
            else if (key == Key.Down || key == Key.S)
                singleplayerViewModel.PlayerOneMoveDirection = GameAssistant.GetUpdatedDirection(playerDir, Direction.South, pressed);
            else if (key == Key.Left || key == Key.A)
                singleplayerViewModel.PlayerOneMoveDirection = GameAssistant.GetUpdatedDirection(playerDir, Direction.West, pressed);
            else if (key == Key.Right || key == Key.D)
                singleplayerViewModel.PlayerOneMoveDirection = GameAssistant.GetUpdatedDirection(playerDir, Direction.East, pressed);
        }

        private void DrawGame()
        {
            SolidColorBrush foregroundBrush = (SolidColorBrush)FindResource("ForegroundBrush");
            GameAssistant.DrawMaze(mainGrid, singleplayerViewModel, foregroundBrush);
            singleplayerViewModel.PlayerOneX = 0;
            singleplayerViewModel.PlayerOneY = 0;
            singleplayerViewModel.PrizeX = singleplayerViewModel.MazeWidth - 1;
            singleplayerViewModel.PrizeY = singleplayerViewModel.MazeHeight - 1;
        }

        protected override void OnClosed(EventArgs e)
        {
            singleplayerViewModel.DisposeTimer();
            base.OnClosed(e);

        }
    }
}
