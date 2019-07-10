using System;
using System.IO;
using System.Numerics;

namespace ConsolePong
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = false;

            try
            {
                Run();
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }

        private static void Run()
        {
            var gamestate = new GameState(new GameSettings()
            {
                FieldSize = new Vector2(10.0f, 2.0f),
                InitialBallVelocity = 0.5f,
                MaxOutgoingAngleToHorizontal = 60.0f,
                VelocityIncrementPerPaddleCollision = 0.1f,
                BallSize = 0.2f,
                PaddleSize = new Vector2(0.2f, 0.5f),
                MaxPaddleVelocity = 1.0f
            });

            var consoleView = new ConsoleView(gamestate.GameSettings);

            var lastFrame = DateTime.Now;

            while (true)
            {
                consoleView.Show(gamestate);
                var now = DateTime.Now;

                var leftPaddleVelocity = 0.0f;
                var rightPaddleVelocity = 0.0f;

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.DownArrow)
                    {
                        leftPaddleVelocity = -0.5f;
                    }
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        leftPaddleVelocity = 0.5f;
                    }
                }

                gamestate.Update(now - lastFrame, leftPaddleVelocity, rightPaddleVelocity);
                lastFrame = now;
            }
        }
    }
}
