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
    /// Interaction logic for MultiplayerWindow.xaml
    /// </summary>
    public partial class MultiplayerWindow : Window
    {

        private readonly MultiplayerViewModel multiplayerViewModel;
        public MultiplayerWindow(Settings settings)
        {

            this.multiplayerViewModel = new MultiplayerViewModel(settings);
            DataContext = this.multiplayerViewModel;
            this.KeyDown += Multiplayer_PreviewKeyDown;
            this.KeyUp += Multiplayer_PreviewKeyUp;

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
            Direction playerTwoDir = multiplayerViewModel.PlayerTwoMoveDirection;
            Direction playerOneDir = multiplayerViewModel.PlayerOneMoveDirection;
            // Update player 2 key states
            if (key == Key.W)
                multiplayerViewModel.PlayerTwoMoveDirection = GameAssistant.GetUpdatedDirection(playerTwoDir, Direction.North, pressed);
            else if (key == Key.S)
                multiplayerViewModel.PlayerTwoMoveDirection = GameAssistant.GetUpdatedDirection(playerTwoDir, Direction.South, pressed);
            else if (key == Key.A)
                multiplayerViewModel.PlayerTwoMoveDirection = GameAssistant.GetUpdatedDirection(playerTwoDir, Direction.West, pressed);
            else if (key == Key.D)
                multiplayerViewModel.PlayerTwoMoveDirection = GameAssistant.GetUpdatedDirection(playerTwoDir, Direction.East, pressed);

            // Update player 1 key states
            else if (key == Key.Up)
                multiplayerViewModel.PlayerOneMoveDirection = GameAssistant.GetUpdatedDirection(playerOneDir, Direction.North, pressed);
            else if (key == Key.Down)
                multiplayerViewModel.PlayerOneMoveDirection = GameAssistant.GetUpdatedDirection(playerOneDir, Direction.South, pressed);
            else if (key == Key.Left)
                multiplayerViewModel.PlayerOneMoveDirection = GameAssistant.GetUpdatedDirection(playerOneDir, Direction.West, pressed);
            else if (key == Key.Right)
                multiplayerViewModel.PlayerOneMoveDirection = GameAssistant.GetUpdatedDirection(playerOneDir, Direction.East, pressed);
        }

        private void DrawGame()
        {
            SolidColorBrush foregroundBrush = (SolidColorBrush)FindResource("ForegroundBrush");
            GameAssistant.DrawMaze(mainGrid, multiplayerViewModel, foregroundBrush);
            multiplayerViewModel.PlayerTwoX = 0;
            multiplayerViewModel.PlayerTwoY = 0;
            multiplayerViewModel.PlayerOneX = multiplayerViewModel.MazeWidth - 1;
            multiplayerViewModel.PlayerOneY = multiplayerViewModel.MazeHeight - 1;
        }
        protected override void OnClosed(EventArgs e)
        {
            multiplayerViewModel.DisposeTimer();
            base.OnClosed(e);

        }
    }
}
