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

        /// <summary>
        /// The Size of the playing field. Defines the area the ball can move in.
        /// </summary>
        public Vector2 FieldSize { get; }

        /// <summary>
        /// The maximum angle to the horizontal the ball direction has after colliding with a paddle.
        /// This angle occurs if the ball hits the topmost or bottommost part of a paddle.
        /// </summary>
        public float MaxAngleToHorizontalAfterPaddleBounceInDegrees { get; }

        /// <summary>
        /// Describes the initial ball velocity at the start of the game.
        /// </summary>
        public float InitialBallVelocity { get; }

        /// <summary>
        /// After each collision with a paddle the velocity increases by this amount.
        /// newVelocity = oldVelocity + VelocityIncrementPerPaddleCollision;
        /// </summary>
        public float VelocityIncrementPerPaddleCollision { get; }

        /// <summary>
        /// The Ball size. This describes the diameter not the radius of the ball.
        /// </summary>
        public float BallSize { get; }

        /// <summary>
        /// The Paddle size. The x value is the length from the left edge to the right edge of the paddle.
        /// The y value is the length from the top edge to the bottom edge of the paddle.
        /// </summary>
        public Vector2 PaddleSize { get; }

        /// <summary>
        /// The maximum velocity of a paddle.
        /// If a controller returns a value bigger than that it is reduced to this amount and then passed to the GameState.Update method.
        /// </summary>
        public float MaxPaddleVelocity { get; }
    }
}