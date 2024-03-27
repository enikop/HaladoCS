using MazeApp.Model;
using MazeApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Documents;

namespace MazeApp.ViewModel
{
    public class MultiplayerViewModel : GameViewModel
    {
        private readonly Player playerOne;
        private readonly Player playerTwo;

        private Position pickupOne;
        private Position pickupTwo;
        private Position pickupMixed;
       
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
            this.pickupOne = GetNewPickupPosition();
            this.pickupTwo = GetNewPickupPosition();
            this.pickupMixed = GetNewPickupPosition();
            SetTimer();

        }

        private Position GetNewPickupPosition()
        {
            Random random = new Random();
            int column = random.Next(0, MazeWidth);
            int row = random.Next(0, MazeHeight);
            Position output = new Position(row, column);
            while(output.Equals(pickupOne) || output.Equals(pickupTwo) 
                || output.Equals(pickupMixed) || output.Equals(playerOne.Position)
                || output.Equals(playerTwo.Position)) 
            {
                output = new Position(random.Next(0, MazeHeight), random.Next(0, MazeWidth));
            }
            return output;
        }

        public override void UpdatePlayerPositions() 
        {
            MovePlayers();
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
            if (playerOne.Position.Equals(pickupOne))
            {
                PlayerOneScore += 1;
                pickupOne = GetNewPickupPosition();
                NotifyPropertyChanged(nameof(PickupOneX));
                NotifyPropertyChanged(nameof(PickupOneY));
            }
            if (playerOne.Position.Equals(pickupMixed))
            {
                PlayerOneScore += 2;
                pickupMixed = GetNewPickupPosition();
                NotifyPropertyChanged(nameof(PickupMixedX));
                NotifyPropertyChanged(nameof(PickupMixedY));
            }
            if (playerTwo.Position.Equals(pickupTwo))
            {
                PlayerTwoScore += 1;
                pickupTwo = GetNewPickupPosition();
                NotifyPropertyChanged(nameof(PickupTwoX));
                NotifyPropertyChanged(nameof(PickupTwoY));
            }
            if (playerTwo.Position.Equals(pickupMixed))
            {
                PlayerTwoScore += 2;
                pickupMixed = GetNewPickupPosition();
                NotifyPropertyChanged(nameof(PickupMixedX));
                NotifyPropertyChanged(nameof(PickupMixedY));
            }
        }
    }
}
