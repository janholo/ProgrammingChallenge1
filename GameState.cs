using System;
using System.Numerics;

namespace ConsolePong
{
    internal class GameState
    {
        public GameState(GameSettings gameSettings)
        {
            GameSettings = gameSettings;

            LeftPaddle = new Paddle(new Vector2(gameSettings.PaddleSize.X, gameSettings.FieldSize.Y * 0.5f));
            RightPaddle = new Paddle(new Vector2(gameSettings.FieldSize.X - gameSettings.PaddleSize.X, gameSettings.FieldSize.Y * 0.5f));

            Ball = new Ball(gameSettings.FieldSize * 0.5f, Helpers.RotatedUnitVector((float)Math.PI / 6.0f) * gameSettings.InitialBallVelocity);
        }

        public GameSettings GameSettings { get; }

        public Paddle LeftPaddle { get; private set; }
        public Paddle RightPaddle { get; private set; }

        public Ball Ball { get; private set; }

        public void Update(TimeSpan deltaTime, float leftPaddleVelocity, float rightPaddleVelocity)
        {
            Console.WriteLine(deltaTime.TotalSeconds);

            var ballPos = Ball.Position + Ball.VelocityInUnitsPerSecond * (float)deltaTime.TotalSeconds;
            Ball = new Ball(ballPos, Ball.VelocityInUnitsPerSecond);

            var leftPaddleVelocityCapped = Math.Min(Math.Max(leftPaddleVelocity, -GameSettings.MaxPaddleVelocity), GameSettings.MaxPaddleVelocity);
            var paddleLeftPos = LeftPaddle.Position + new Vector2(0.0f, leftPaddleVelocityCapped * (float)deltaTime.TotalSeconds);
            LeftPaddle = new Paddle(paddleLeftPos);

            var rightPaddleVelocityCapped = Math.Min(Math.Max(rightPaddleVelocity, -GameSettings.MaxPaddleVelocity), GameSettings.MaxPaddleVelocity);
            var paddleRightPos = RightPaddle.Position + new Vector2(0.0f, rightPaddleVelocityCapped * (float)deltaTime.TotalSeconds);
            RightPaddle = new Paddle(paddleRightPos);


        }
    }
}