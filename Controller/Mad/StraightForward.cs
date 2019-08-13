using ConsolePong.GameLogic;
using System;
using System.Diagnostics;
using System.Numerics;

namespace ConsolePong.Controller.Mad
{
    internal class StraightForward : IController
    {
        private Vector2? previousBallPos;

        public float Update(GameState gameState, Player player)
        {   
            bool isLeftPlayer = player == Player.Left;
            var myPaddle = isLeftPlayer ? gameState.LeftPaddle : gameState.RightPaddle;
            // simulate 4 frames
            var currentFrametime = TimeSpan.FromSeconds(1.0f / Math.Max(gameState.Fps, 10f));
            var currentBall = gameState.Ball;
            var (nextBall, hitRightWall, hitLeftWall) = SimulateN(4, gameState.GameSettings, currentFrametime, gameState.Ball);
            if (currentBall.Position.X < nextBall.Position.X && !isLeftPlayer || currentBall.Position.X > nextBall.Position.X && isLeftPlayer) {
                // ball going to the right, and we are the right player || going to the left and we are the left player
                // approximate target position and move there, because of the 90 degree stuff this is a bit unfortunate to calculate :(
                // So just simulate again
                
                if (hitRightWall || hitLeftWall) {
                    // no need to simulate
                    // account for paddle width
                    Debug.WriteLine($"Expect to hit in {nextBall.Position.Y}, currently in {myPaddle.Position.Y}");
                    return MoveTo(myPaddle, nextBall.Position.Y, gameState.GameSettings);
                }

                var simulateTime = TimeSpan.FromSeconds(Math.Max(currentFrametime.TotalSeconds, 0.01f)); // simulate at least 10ms
                var (nextBallS, hitRightWallS, hitLeftWallS) = SimulateUntilLeftOrRight(gameState.GameSettings, simulateTime, gameState.Ball);
                Debug.Assert(hitLeftWallS && isLeftPlayer || hitRightWallS && !isLeftPlayer, "Expected proper simulation");
                
                Debug.WriteLine($"Expect to hit in {nextBallS.Position.Y}, currently in {myPaddle.Position.Y}");
                return MoveTo(myPaddle, nextBallS.Position.Y, gameState.GameSettings);
            } else {
                // ball going to the other side -> go to the middle
                // We don't stop which probably looks weird but thats ok
                var paddleMiddleYPos = myPaddle.Position.Y;
                return MoveTo(myPaddle, gameState.GameSettings.FieldSize.Y * 0.5f, gameState.GameSettings);
            }

            return MoveTo(myPaddle, gameState.Ball.Position.Y, gameState.GameSettings);
        }

        public float MoveTo(Paddle myPaddle, float y, GameSettings settings)
        {
            var middlePaddleY = myPaddle.Position.Y - settings.PaddleSize.Y * 0.5f;
            var dist = middlePaddleY - y;
            var absDist = Math.Abs(dist);
            var velocity = settings.MaxPaddleVelocity;
            //Console.WriteLine($"mypaddle: {myPaddle.Position.X}, {myPaddle.Position.Y}");
            if (absDist < settings.BallSize * 0.5) {
                while (absDist < velocity && velocity > 0.5) {
                    velocity = velocity * 0.5f;
                }
            }
            
            if (dist < 0)
            {
                return velocity;
            }
            else
            {
                return -velocity;
            }
        }

        public (Ball nextBall, bool hitRightWall, bool hitLeftWall) SimulateUntilLeftOrRight(GameSettings GameSettings, TimeSpan deltaTime, Ball Ball)
        {
            Ball currentBall = Ball;
            while (true)
            {
                var (nextBall, hitRightWall, hitLeftWall) = SimulateNext(GameSettings, deltaTime, currentBall);
                if (hitRightWall || hitLeftWall){
                    return (nextBall, hitRightWall, hitLeftWall);
                }else{
                    currentBall = nextBall;
                }
            }

            return (currentBall, false, false);
        }

        public (Ball nextBall, bool hitRightWall, bool hitLeftWall) SimulateN(int n, GameSettings GameSettings, TimeSpan deltaTime, Ball Ball)
        {
            Ball currentBall = Ball;
            for (int i = 0; i < n; i++)
            {
                var (nextBall, hitRightWall, hitLeftWall) = SimulateNext(GameSettings, deltaTime, currentBall);
                if (hitRightWall){
                    return (nextBall, hitRightWall, hitLeftWall);
                }else if(hitLeftWall){
                    return (nextBall, hitRightWall, hitLeftWall);
                }else{
                    currentBall = nextBall;
                }
            }

            return (currentBall, false, false);
        }

        public (Ball nextBall, bool hitRightWall, bool hitLeftWall) SimulateNext(GameSettings GameSettings, TimeSpan deltaTime, Ball Ball)
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
            // We need to account for the paddle width as we are only interested in that
            var hitRightWall = false;
            var hitLeftWall = false;
            var ball = new Ball(ballPos, ballVelocity);
            if (ballPos.X > (GameSettings.FieldSize.X - GameSettings.PaddleSize.X) - GameSettings.BallSize * 0.5f)
            {
                hitRightWall = true;
            }
            else if (ballPos.X < GameSettings.BallSize * 0.5f + GameSettings.PaddleSize.X)
            {
                hitLeftWall = true;
            }

            return (ball, hitRightWall, hitLeftWall);
        }

        public string Name => nameof(StraightForward);
    }
}