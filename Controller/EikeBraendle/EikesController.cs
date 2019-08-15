using ConsolePong.GameLogic;
using System;
using System.Numerics;

namespace ConsolePong.Controller.EikeBraendle
{
    internal class EikesController : IController
    {
        private Vector2 currentEndPosition;
        private bool calculateEndPosition;
        private Vector2 _lastVelocity;

        public float Update(GameState gameState, Player player)
        {
            var myPaddle = player == Player.Left ? gameState.LeftPaddle : gameState.RightPaddle;
            var isMyPaddleLeft = player == Player.Left ? true : false;

            if (isMyPaddleLeft)
            {
                return PlayLeft(gameState, myPaddle, isMyPaddleLeft);
            }
            else
            {
                return PlayRight(gameState, myPaddle, isMyPaddleLeft);
            }
        }

        private float PlayRight(GameState gameState, Paddle myPaddle, bool isMyPaddleLeft)
        {
            var velocity = gameState.Ball.VelocityInUnitsPerSecond;
            var position = gameState.Ball.Position;

            if (_lastVelocity.X <= 0 && velocity.X > 0)
            {
                calculateEndPosition = true;
            }
            else
            {
                calculateEndPosition = false;
            }


            _lastVelocity = velocity;

            if (velocity.X < 0)
            {
                if (myPaddle.Position.Y >= (gameState.GameSettings.FieldSize.Y / 2))
                {
                    return -gameState.GameSettings.MaxPaddleVelocity;
                }

                if (myPaddle.Position.Y <= (gameState.GameSettings.FieldSize.Y / 2))
                {
                    return gameState.GameSettings.MaxPaddleVelocity;
                }

                return 0;
            }

            if (calculateEndPosition)
            {
                currentEndPosition = CalculatePosition(gameState, isMyPaddleLeft);
            }

            var desiredPosition = 0f;
            if (velocity.X < 10)
            {
                desiredPosition = 0.05f;
            }

            if (currentEndPosition.Y > (myPaddle.Position.Y + desiredPosition))
            {
                return gameState.GameSettings.MaxPaddleVelocity;
            }

            if (currentEndPosition.Y < (myPaddle.Position.Y - desiredPosition))
            {
                return -gameState.GameSettings.MaxPaddleVelocity;
            }

            return 0;
        }

        private float PlayLeft(GameState gameState, Paddle myPaddle, bool isMyPaddleLeft)
        {
            var velocity = gameState.Ball.VelocityInUnitsPerSecond;
            var position = gameState.Ball.Position;

            if (_lastVelocity.X > 0 && velocity.X < 0)
            {
                calculateEndPosition = true;
            }
            else
            {
                calculateEndPosition = false;
            }


            _lastVelocity = velocity;

            if (velocity.X > 0)
            {
                if (myPaddle.Position.Y >= (gameState.GameSettings.FieldSize.Y / 2))
                {
                    return -gameState.GameSettings.MaxPaddleVelocity;
                }

                if (myPaddle.Position.Y <= (gameState.GameSettings.FieldSize.Y / 2))
                {
                    return gameState.GameSettings.MaxPaddleVelocity;
                }

                return 0;
            }

            if (calculateEndPosition)
            {
                currentEndPosition = CalculatePosition(gameState, isMyPaddleLeft);
            }

            var desiredPosition = 0f;
            if (velocity.X > -10)
            {
                desiredPosition = 0.05f;
            }

            if (currentEndPosition.Y > (myPaddle.Position.Y + desiredPosition))
            {
                return gameState.GameSettings.MaxPaddleVelocity;
            }

            if (currentEndPosition.Y < (myPaddle.Position.Y - desiredPosition))
            {
                return -gameState.GameSettings.MaxPaddleVelocity;
            }

            return 0;
        }

        private Vector2 CalculatePosition(GameState gameState, bool isMyPaddleLeft)
        {
            var hit = false;
            var ballPosition = gameState.Ball.Position;
            var ballVelocity = gameState.Ball.VelocityInUnitsPerSecond;
            var currentFrametime = TimeSpan.FromSeconds(1.0f / Math.Max(gameState.Fps, 10f));

            while (!hit)
            {
                ballPosition = ballPosition + ballVelocity * (float)currentFrametime.TotalSeconds;

                if (ballPosition.Y > gameState.GameSettings.FieldSize.Y - gameState.GameSettings.BallSize * 0.5f)
                {
                    ballPosition.Y = gameState.GameSettings.FieldSize.Y - gameState.GameSettings.BallSize * 0.5f;
                    ballVelocity.Y = -ballVelocity.Y;
                }
                else if (ballPosition.Y < gameState.GameSettings.BallSize * 0.5f)
                {
                    ballPosition.Y = gameState.GameSettings.BallSize * 0.5f;
                    ballVelocity.Y = -ballVelocity.Y;
                }

                if (ballPosition.X > gameState.GameSettings.FieldSize.X - gameState.GameSettings.BallSize * 0.5f)
                {
                    hit = true;
                }
                else if (ballPosition.X < gameState.GameSettings.BallSize * 0.5f)
                {
                    hit = true;
                }
            }

            return ballPosition;

        }

        public string Name => nameof(EikesController);
    }
}