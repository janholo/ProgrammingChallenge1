using ConsolePong.GameLogic;

namespace ConsolePong.Controller.EikeBraendle
{
    internal class EikesController : IController
    {
        public float Update(GameState gameState, Player player)
        {
            var myPaddle = player == Player.Left ? gameState.LeftPaddle : gameState.RightPaddle;

            var velocity = gameState.Ball.VelocityInUnitsPerSecond;
            var position = gameState.Ball.Position;

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

            if (gameState.Ball.Position.Y > (myPaddle.Position.Y + 0.1))
            {
                return gameState.GameSettings.MaxPaddleVelocity;
            }

            if (gameState.Ball.Position.Y < (myPaddle.Position.Y - 0.1))
            {
                return -gameState.GameSettings.MaxPaddleVelocity;
            }

            return 0;
        }

        public string Name => nameof(EikesController);
    }
}