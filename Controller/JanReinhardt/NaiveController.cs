using ConsolePong.GameLogic;

namespace ConsolePong.Controller.JanReinhardt
{
    internal class NaiveController : IController
    {
        public float Update(GameState gameState, Player player)
        {
            var myPaddle = player == Player.Left ? gameState.LeftPaddle : gameState.RightPaddle;

            if (gameState.Ball.Position.Y > myPaddle.Position.Y)
            {
                return gameState.GameSettings.MaxPaddleVelocity;
            }
            else
            {
                return -gameState.GameSettings.MaxPaddleVelocity;
            }
        }

        public string Name => nameof(NaiveController);
    }
}