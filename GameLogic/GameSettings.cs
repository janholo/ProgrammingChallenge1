using System.Numerics;

namespace ConsolePong.GameLogic
{
    internal struct GameSettings
    {
        public Vector2 FieldSize { get; set; }

        public float MaxOutgoingAngleToHorizontal { get; set; }

        public float InitialBallVelocity { get; set; }

        public float VelocityIncrementPerPaddleCollision { get; set; }

        public float BallSize { get; set; }

        public Vector2 PaddleSize { get; set; }
        public float MaxPaddleVelocity { get; set; }
    }
}