using System.Numerics;

namespace ConsolePong.GameLogic
{
    internal class GameSettings
    {
        public GameSettings(Vector2 fieldSize, float maxAngleToHorizontalAfterPaddleBounceInDegrees, float initialBallVelocity, float velocityIncrementPerPaddleCollision, float ballSize, Vector2 paddleSize, float maxPaddleVelocity)
        {
            FieldSize = fieldSize;
            MaxAngleToHorizontalAfterPaddleBounceInDegrees = maxAngleToHorizontalAfterPaddleBounceInDegrees;
            InitialBallVelocity = initialBallVelocity;
            VelocityIncrementPerPaddleCollision = velocityIncrementPerPaddleCollision;
            BallSize = ballSize;
            PaddleSize = paddleSize;
            MaxPaddleVelocity = maxPaddleVelocity;
        }

        public Vector2 FieldSize { get; }

        public float MaxAngleToHorizontalAfterPaddleBounceInDegrees { get; }

        public float InitialBallVelocity { get; }

        public float VelocityIncrementPerPaddleCollision { get; }

        public float BallSize { get; }

        public Vector2 PaddleSize { get; }

        public float MaxPaddleVelocity { get; }
    }
}