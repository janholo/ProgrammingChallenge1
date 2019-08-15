using ConsolePong.GameLogic;
using ConsolePong.View;
using System;
using System.Numerics;

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
            Console.WriteLine("Resize your terminal and press any key if you are ready!");
            Console.ReadKey();

            var gameSettings = new GameSettings(
                new Vector2(18.0f, 10.0f),
                60.0f,
                10.0f,
                1.0f,
                0.2f,
                new Vector2(0.2f, 0.5f),
                4.0f
            );

            var consoleView = new ConsoleView(gameSettings);

            var runner = new GameRunner();

            //var rightController = new Controller.EikeBraendle.EikesController();
            //var leftController = new Controller.Mad.StraightForward();

            var leftController = new Controller.EikeBraendle.EikesController();
            var rightController = new Controller.Mad.StraightForward();


            runner.Run(gameSettings, consoleView, leftController, rightController);

            Console.WriteLine("Press any key to exit:");
            Console.ReadKey();
        }
    }
}
