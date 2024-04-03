using MazeApp.Model;
using MazeApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Timers;
using System.Windows.Documents;

namespace MazeApp.ViewModel
{
    public class MultiplayerViewModel : GameViewModel
    {
        private readonly int winningScore = 15;
        private int? winnerNum = null;

        private readonly Player playerOne;
        private readonly Player playerTwo;

        private Pickup pickupOne;
        private Pickup pickupTwo;
        private Pickup pickupMixed;

        public bool HasGameEnded {
            get
            {
                return winnerNum != null;
            }
        }

        public int? WinnerNum
        {
            get
            {
                return winnerNum;
            }
            set
            {
                winnerNum = value;
                NotifyPropertyChanged(nameof(WinnerNum));
                NotifyPropertyChanged(nameof(HasGameEnded));
            }
        }
       
        public int PickupOneX
        {
            get { return pickupOne.Column; }
            set 
            { 
                pickupOne.Column = value;
                NotifyPropertyChanged(nameof(PickupOneX));
            }
        }

        public int PickupOneY
        {
            get { return pickupOne.Row; }
            set
            {
                pickupOne.Row = value;
                NotifyPropertyChanged(nameof(PickupOneY));
            }
        }

        public int PickupTwoX
        {
            get { return pickupTwo.Column; }
            set
            {
                pickupTwo.Column = value;
                NotifyPropertyChanged(nameof(PickupTwoX));
            }
        }

        public int PickupTwoY
        {
            get { return pickupTwo.Row; }
            set
            {
                pickupTwo.Row = value;
                NotifyPropertyChanged(nameof(PickupTwoY));
            }
        }

        public int PickupMixedX
        {
            get { return pickupMixed.Column; }
            set
            {
                pickupMixed.Column = value;
                NotifyPropertyChanged(nameof(PickupMixedX));
            }
        }

        public int PickupMixedY
        {
            get { return pickupMixed.Row; }
            set
            {
                pickupMixed.Row = value;
                NotifyPropertyChanged(nameof(PickupMixedY));
            }
        }

        public int PickupSize
        {
            get { return CellSize / 2; }
        }

        public int PlayerOneScore
        {
            get {
                return playerOne.Score;
            }
            set
            {
               playerOne.Score = value;
               NotifyPropertyChanged(nameof(PlayerOneScore));
            }
        }

        public int PlayerTwoScore
        {
            get
            {
                return playerTwo.Score;
            }
            set
            {
                playerTwo.Score = value;
                NotifyPropertyChanged(nameof(PlayerTwoScore));
            }
        }

        public Direction PlayerOneMoveDirection
        {
            get
            {
                return playerOne.MoveDirection;
            }
            set
            {
                playerOne.MoveDirection = value;
                NotifyPropertyChanged(nameof(PlayerOneMoveDirection));
            }
        }

        public Direction PlayerTwoMoveDirection
        {
            get
            {
                return playerTwo.MoveDirection;
            }
            set
            {
                playerTwo.MoveDirection = value;
                NotifyPropertyChanged(nameof(PlayerTwoMoveDirection));
            }
        }

        public int PlayerOneX
        {
            get { return playerOne.Position.Column; }
            set { 
                playerOne.Position.Column = value; 
                NotifyPropertyChanged(nameof(PlayerOneX));
            }
        }

        public int PlayerOneY
        {
            get { return playerOne.Position.Row; }
            set { 
                playerOne.Position.Row = value; 
                NotifyPropertyChanged(nameof(PlayerOneY));
            }
        }

        public int PlayerTwoX
        {
            get { return playerTwo.Position.Column; }
            set { 
                playerTwo.Position.Column = value; 
                NotifyPropertyChanged(nameof(PlayerTwoX));
            }
        }

        public int PlayerTwoY
        {
            get { return playerTwo.Position.Row; }
            set { 
                playerTwo.Position.Row = value;
                NotifyPropertyChanged(nameof(PlayerTwoY));
            }
        }

        public MultiplayerViewModel(Settings settings): base(settings)
        {
            this.playerOne = new Player();
            this.playerTwo = new Player();
            this.pickupOne = new Pickup();
            this.pickupTwo = new Pickup();
            this.pickupMixed = new Pickup();

            pickupOne.Position = GetNewPickupPosition();
            pickupTwo.Position = GetNewPickupPosition();
            pickupMixed.Position = GetNewPickupPosition();
            SetTimer();

        }

        private Position GetNewPickupPosition()
        {
            Random random = new Random();
            Position output;
            do
            {
                int column = random.Next(0, MazeWidth);
                int row = random.Next(0, MazeHeight);
                output = new Position(row, column);
            }
            while (output.Equals(pickupOne.Position) || output.Equals(pickupTwo.Position)
                || output.Equals(pickupMixed.Position) || output.Equals(playerOne.Position)
                || output.Equals(playerTwo.Position));
            return output;
        }

        public override void UpdatePlayerPositions() 
        {
            if (!HasGameEnded)
            {
                MovePlayers();
            }
        }

        public void MovePlayers()
        {
            Direction[] dirs = new Direction[] { Direction.North, Direction.South, Direction.West, Direction.East };
            foreach (Direction dir in dirs)
            {
                if (playerOne.MoveDirection.HasFlag(dir))
                {
                    Position newPos = playerOne.PreviewStepOne(dir);
                    if(IsWithinBoundaries(newPos) && !maze.IsWallBetween(playerOne.Position, newPos))
                    {
                        PlayerOneX = newPos.Column;
                        PlayerOneY = newPos.Row;
                    }

                }
                if (playerTwo.MoveDirection.HasFlag(dir))
                {
                    Position newPos = playerTwo.PreviewStepOne(dir);
                    if (IsWithinBoundaries(newPos) && !maze.IsWallBetween(playerTwo.Position, newPos))
                    {
                        PlayerTwoX = newPos.Column;
                        PlayerTwoY = newPos.Row;
                    }
                }
                HandlePickups();
            }
        }

        private void HandlePickups()
        {
            if (playerOne.Position.Equals(pickupOne.Position))
            {
                PlayerOneScore += 1;
                pickupOne.IsToMove = true;
            }
            else if (playerOne.Position.Equals(pickupMixed.Position))
            {
                PlayerOneScore += 2;
                pickupMixed.IsToMove = true;
            }


            if (playerTwo.Position.Equals(pickupTwo.Position))
            {
                PlayerTwoScore += 1;
                pickupTwo.IsToMove = true;
            }
            else if (playerTwo.Position.Equals(pickupMixed.Position))
            {
                PlayerTwoScore += 2;
                pickupMixed.IsToMove = true;
            }

            MovePickups();
            CheckForWin();
        }

        private void MovePickups()
        {
            Position newPos;
            if (pickupOne.IsToMove)
            {
                newPos = GetNewPickupPosition();
                PickupOneX = newPos.Column;
                PickupOneY = newPos.Row;
            }

            if (pickupTwo.IsToMove)
            {
                newPos = GetNewPickupPosition();
                PickupTwoX = newPos.Column;
                PickupTwoY = newPos.Row;
            }

            if (pickupMixed.IsToMove)
            {
                newPos = GetNewPickupPosition();
                PickupMixedX = newPos.Column;
                PickupMixedY = newPos.Row;
            }
        }

        private void CheckForWin()
        {
            if (PlayerOneScore >= winningScore && PlayerTwoScore >= winningScore)
            {
                WinnerNum = 0;
            }
            else if (PlayerOneScore >= winningScore)
            {
                WinnerNum = 1;
            }
            else if (PlayerTwoScore >= winningScore)
            {
                WinnerNum = 2;
            }
        }
    }
}
