using System;
using ConsolePong.Controller;
using ConsolePong.View;

namespace ConsolePong.GameLogic
{
    internal class GameRunner
    {
        public void Run(GameSettings gameSettings, IView view, IController leftController, IController rightController)
        {
            var gameState = new GameState(gameSettings);

            var lastFrame = DateTime.Now;

            while (true)
            {
                view.Show(gameState, leftController.Name, rightController.Name);
                var now = DateTime.Now;
                var deltaTime = now - lastFrame;
                lastFrame = now;

                float leftPaddleVelocity = 0.0f;
                try
                {
                    leftPaddleVelocity = leftController.Update(gameState, Player.Left);
                }
                catch (Exception)
                {
                    // Update method failed. dont move the paddle
                }

                float rightPaddleVelocity = 0.0f;
                try
                {
                    rightPaddleVelocity = rightController.Update(gameState, Player.Right);
                }
                catch (Exception)
                {
                    // Update method failed. dont move the paddle
                }
                
                gameState = gameState.Update(deltaTime, leftPaddleVelocity, rightPaddleVelocity);

                if (gameState.GameResult != GameResult.Pending)
                {
                    break;
                }
            }

            view.Show(gameState, leftController.Name, rightController.Name);
        }
    }
}
