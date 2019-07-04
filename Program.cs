using System;
using System.IO;
using System.Numerics;

namespace ConsolePong
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gamestate = new GameState(new GameSettings()
            {
                FieldSize = new Vector2(10.0f, 2.0f),
                InitialBallVelocity = 0.1f,
                MaxOutgoingAngleToHorizontal = 60.0f,
                VelocityIncrementPerPaddleCollision = 0.1f,
                BallSize = 0.2f,
                PaddleSize = new Vector2(0.2f, 0.5f),
                MaxPaddleVelocity = 1.0f
            });

            var consoleFieldWidth = 80;
            try
            {
                consoleFieldWidth = Console.WindowWidth - 1;
            }
            catch (IOException)
            {
                Console.WriteLine($"Cannot get Console.WindowWidth -> use default width of {consoleFieldWidth}");
            }


            var consoleView = new ConsoleView(consoleFieldWidth, gamestate.GameSettings);

            var lastFrame = DateTime.Now;

            while(true)
            {
                consoleView.Show(gamestate);
                var now = DateTime.Now;

                var leftPaddleVelocity = 0.0f;
                var rightPaddleVelocity = 0.0f;

                if(Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    Console.WriteLine(key.Key);
                    if(key.Key == ConsoleKey.DownArrow)
                    {
                        leftPaddleVelocity = -0.5f;
                    }
                    if(key.Key == ConsoleKey.UpArrow)
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
