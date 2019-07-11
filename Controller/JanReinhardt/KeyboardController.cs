using System;
using ConsolePong.GameLogic;

namespace ConsolePong.Controller.JanReinhardt
{
    internal class KeyboardController : IController
    {
        public float Update(GameState gameState, Player player)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.DownArrow)
                {
                    return -gameState.GameSettings.MaxPaddleVelocity;
                }
                if (key.Key == ConsoleKey.UpArrow)
                {
                    return gameState.GameSettings.MaxPaddleVelocity;
                }
            }

            return 0.0f;
        }

        public string Name => nameof(KeyboardController);
    }
}