using System;
using System.Numerics;

namespace ConsolePong.GameLogic
{
    internal class GameState
    {
        public GameState(GameSettings gameSettings)
        {
            GameSettings = gameSettings;

            LeftPaddle = new Paddle(new Vector2(gameSettings.PaddleSize.X, gameSettings.FieldSize.Y * 0.5f));
            RightPaddle = new Paddle(new Vector2(gameSettings.FieldSize.X - gameSettings.PaddleSize.X, gameSettings.FieldSize.Y * 0.5f));

            Ball = new Ball(gameSettings.FieldSize * 0.5f, Helpers.RotatedUnitVector((float)Math.PI / 18.0f) * gameSettings.InitialBallVelocity);

            GameResult = GameResult.Pending;
        }

        public GameState(GameSettings gameSettings, GameResult result, Paddle leftPaddle, Paddle rightPaddle, Ball ball, float fps)
        {
            GameSettings = gameSettings;
            GameResult = result;
            LeftPaddle = leftPaddle;
            RightPaddle = rightPaddle;
            Ball = ball;
            Fps = fps;
        }

        /// <summary>
        /// All the static settings of the game, they do not change during the run of a game
        /// </summary>
        public GameSettings GameSettings { get; }

        /// <summary>
        /// If the game is still running the GameResult is Pending.
        /// </summary>
        public GameResult GameResult { get; }

        /// <summary>
        /// The Paddle of the left side of the playing field
        /// </summary>
        public Paddle LeftPaddle { get; }


        /// <summary>
        /// The Paddle of the right side of the playing field
        /// </summary>
        public Paddle RightPaddle { get; }

        /// <summary>
        /// The Ball
        /// </summary>
        public Ball Ball { get; }

        /// <summary>
        /// The Fps with which this game is run.
        /// </summary>
        public float Fps { get; }
        
        /// <summary>
        /// Advances the game once step
        /// </summary>
        /// <param name="deltaTime">Should be the time between the last Update call and now</param>
        /// <param name="leftPaddleVelocity">Velocity of the left paddle, should be supplied by a controller</param>
        /// <param name="rightPaddleVelocity">Velocity of the right paddle, should be supplied by a controller</param>
        /// <returns></returns>
        public GameState Update(TimeSpan deltaTime, float leftPaddleVelocity, float rightPaddleVelocity)
        {
            if(GameResult != GameResult.Pending)
            {
                return this;
            }

            var currentFps = 1.0f / (float)deltaTime.TotalSeconds;
            var newFps = Fps * 0.9f + currentFps * 0.1f;

            var newLeftPaddle = UpdatePaddle(deltaTime, LeftPaddle, leftPaddleVelocity);
            var newRightPaddle = UpdatePaddle(deltaTime, RightPaddle, rightPaddleVelocity);

            var (newBall, newGameResult) = UpdateBall(deltaTime);

            return new GameState(GameSettings, newGameResult, newLeftPaddle, newRightPaddle, newBall, newFps);
        }

        private (Ball, GameResult) UpdateBall(TimeSpan deltaTime)
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

            // check left and right wall if game is over
            if (ballPos.X > GameSettings.FieldSize.X - GameSettings.BallSize * 0.5f)
            {
                return (Ball, GameResult.LeftPlayerWon);
            }
            else if (ballPos.X < GameSettings.BallSize * 0.5f)
            {
                return (Ball, GameResult.RightPlayerWon);
            }

            // check collision with paddles
            if (BallCollidesWithPaddle(ballPos, LeftPaddle))
            {
                ballPos.X = LeftPaddle.Position.X + (GameSettings.PaddleSize.X + GameSettings.BallSize) * 0.5f;
                ballVelocity = CalcVelocityAfterBounce(LeftPaddle);
            }
            else if (BallCollidesWithPaddle(ballPos, RightPaddle))
            {
                ballPos.X = RightPaddle.Position.X - (GameSettings.PaddleSize.X + GameSettings.BallSize) * 0.5f;
                ballVelocity = CalcVelocityAfterBounce(RightPaddle);
            }

            var ball = new Ball(ballPos, ballVelocity);
            return (ball, GameResult.Pending);
        }

        private Vector2 CalcVelocityAfterBounce(Paddle paddle)
        {
            var newVelocity = Ball.VelocityInUnitsPerSecond.Length() + GameSettings.VelocityIncrementPerPaddleCollision;
            var offsetToPaddle = (Ball.Position.Y - paddle.Position.Y) / ((GameSettings.PaddleSize.Y + GameSettings.BallSize) * 0.5f);
            var angleInRadians = offsetToPaddle * GameSettings.MaxAngleToHorizontalAfterPaddleBounceInDegrees * (float)Math.PI / 180.0f;

            var velocity = Helpers.RotatedUnitVector(angleInRadians) * newVelocity;

            // A small hack to mirror the velocity if this is the right paddle
            if (paddle == RightPaddle)
            {
                velocity.X = -velocity.X;
            }

            return velocity;
        }

        private bool BallCollidesWithPaddle(Vector2 ballPos, Paddle paddle)
        {
            // Ball is assumed a square for this collision detection
            if (Math.Abs(ballPos.X - paddle.Position.X) < (GameSettings.BallSize + GameSettings.PaddleSize.X) * 0.5f)
            {
                if (Math.Abs(ballPos.Y - paddle.Position.Y) < (GameSettings.BallSize + GameSettings.PaddleSize.Y) * 0.5f)
                {
                    return true;
                }
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