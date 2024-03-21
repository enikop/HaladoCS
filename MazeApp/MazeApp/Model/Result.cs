using MazeApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MazeApp.Model
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public string PlayerName { get; set; }
        [DataMember]
        public int ElapsedTime { get; set; }
        [DataMember]
        public string Algorithm { get; set; }
        [DataMember]
        public int MazeWidth { get; set; }
        [DataMember]
        public int MazeHeight { get; set; }
        [DataMember]
        public bool IsLimitedVisibility { get; set; }

        public Result()
        {
            PlayerName = "Player";
            ElapsedTime = 1000;
            Algorithm = (GenerationAlgorithm.Wilson).ToString();
            MazeWidth = 16;
            MazeHeight = 16;
            IsLimitedVisibility = true;
        }
        public Result(string playerName, int elapsedTime, GenerationAlgorithm generationAlgorithm, int mazeWidth, int mazeHeight, bool isLimitedVisibility)
        {
            PlayerName = playerName;
            ElapsedTime = elapsedTime;
            Algorithm = generationAlgorithm.ToString();
            MazeWidth = mazeWidth;
            MazeHeight = mazeHeight;
            IsLimitedVisibility = isLimitedVisibility;
        }

        public bool IsSameCategory(Result other)
        {
            return Algorithm == other.Algorithm &&
                  MazeWidth == other.MazeWidth &&
                  MazeHeight == other.MazeHeight &&
                  IsLimitedVisibility == other.IsLimitedVisibility;
        }

        public override bool Equals(object? obj)
        {
            return obj is Result result &&
                   PlayerName == result.PlayerName &&
                   ElapsedTime == result.ElapsedTime &&
                   Algorithm == result.Algorithm &&
                   MazeWidth == result.MazeWidth &&
                   MazeHeight == result.MazeHeight &&
                   IsLimitedVisibility == result.IsLimitedVisibility;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerName, ElapsedTime, Algorithm, MazeWidth, MazeHeight, IsLimitedVisibility);
        }
    }
}
