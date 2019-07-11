using System;
using System.Numerics;
using ConsolePong.GameLogic;
using ConsolePong.View;

namespace ConsolePong
{
    internal class Program
    {
        static void Main()
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
            var gameSettings = new GameSettings()
            {
                FieldSize = new Vector2(10.0f, 2.0f),
                InitialBallVelocity = 0.5f,
                MaxOutgoingAngleToHorizontal = 60.0f,
                VelocityIncrementPerPaddleCollision = 0.1f,
                BallSize = 0.2f,
                PaddleSize = new Vector2(0.2f, 0.5f),
                MaxPaddleVelocity = 4.0f
            };

            var consoleView = new ConsoleView(gameSettings);

            var runner = new GameRunner();

            runner.Run(gameSettings, consoleView, new Controller.JanReinhardt.NaiveController(), new Controller.JanReinhardt.KeyboardController());

            Console.WriteLine("Press any key to exit:");
            Console.ReadKey();
        }
    }
}
