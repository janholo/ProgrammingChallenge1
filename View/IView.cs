using ConsolePong.GameLogic;

namespace ConsolePong.View
{
    internal interface IView
    {
        void Show(GameState gameState, string leftControllerName, string rightControllerName);
    }
}