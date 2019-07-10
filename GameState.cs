using System;
using System.Numerics;

namespace ConsolePong
{
    internal class GameState
    {
        public enum GameResult
        {
            Pending,
            LeftPlayerWon,
            RightPlayerWon

        }

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

        public float Fps { get; private set; }


        public void Update(TimeSpan deltaTime, float leftPaddleVelocity, float rightPaddleVelocity)
        {
            var currentFps = 1.0f / (float)deltaTime.TotalSeconds;
            Fps = Fps * 0.9f + currentFps * 0.1f;

            UpdateBall(deltaTime);

            LeftPaddle = UpdatePaddle(deltaTime, LeftPaddle, leftPaddleVelocity);
            RightPaddle = UpdatePaddle(deltaTime, RightPaddle, rightPaddleVelocity);
        }

        private GameResult UpdateBall(TimeSpan deltaTime)
        {
            var ballPos = Ball.Position + Ball.VelocityInUnitsPerSecond * (float)deltaTime.TotalSeconds;
            var ballVelocity = Ball.VelocityInUnitsPerSecond;

            // check collision with upper/lower wall
            if (ballPos.Y > GameSettings.FieldSize.Y - GameSettings.BallSize * 0.5f)
            {
                ballPos.Y = GameSettings.FieldSize.Y - GameSettings.BallSize * 0.5f;
                ballVelocity.Y = -ballVelocity.Y;
            }
            else if (ballPos.Y < GameSettings.BallSize * 0.5f)
            {
                ballPos.Y = GameSettings.BallSize * 0.5f;
                ballVelocity.Y = -ballVelocity.Y;
            }

            // check left and right wall if game os over
            if (ballPos.X > GameSettings.FieldSize.X - GameSettings.BallSize * 0.5f)
            {
                return GameResult.LeftPlayerWon;
            }
            else if (ballPos.Y < GameSettings.BallSize * 0.5f)
            {
                return GameResult.RightPlayerWon;
            }

            // check collision with paddles
            if(BallCollidesWithPaddle(ballPos, LeftPaddle))
            {
                ballPos.X = LeftPaddle.Position.X + (GameSettings.PaddleSize.X + GameSettings.BallSize) * 0.5f;
                ballVelocity.X = ballVelocity.X < 0.0f ? -ballVelocity.X : ballVelocity.X;
            }
            else if(BallCollidesWithPaddle(ballPos, RightPaddle))
            {
                ballPos.X = RightPaddle.Position.X - (GameSettings.PaddleSize.X + GameSettings.BallSize) * 0.5f;
                ballVelocity.X = ballVelocity.X > 0.0f ? -ballVelocity.X : ballVelocity.X;                
            }

            Ball = new Ball(ballPos, ballVelocity);
            return GameResult.Pending;
        }

        private bool BallCollidesWithPaddle(Vector2 ballPos, Paddle paddle)
        {
            // Ball is assumed a square for this collision detection
            if(Math.Abs(ballPos.X - paddle.Position.X) < (GameSettings.BallSize + GameSettings.PaddleSize.X) * 0.5f)
            {
                return true;
            }

            return false;
        }

        private Paddle UpdatePaddle(TimeSpan deltaTime, Paddle paddle, float paddleVelocity)
        {
            var paddleVelocityCapped = Math.Min(Math.Max(paddleVelocity, -GameSettings.MaxPaddleVelocity), GameSettings.MaxPaddleVelocity);
            var paddlePos = paddle.Position + new Vector2(0.0f, paddleVelocityCapped * (float)deltaTime.TotalSeconds);

            paddlePos.Y = Math.Min(Math.Max(paddlePos.Y, GameSettings.PaddleSize.Y * 0.5f), GameSettings.FieldSize.Y - GameSettings.PaddleSize.Y * 0.5f);

            return new Paddle(paddlePos);
        }
    }
}