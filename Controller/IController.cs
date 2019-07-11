using ConsolePong.GameLogic;

namespace ConsolePong.Controller
{
    internal interface IController
    {
        float Update(GameState gameState, Player player);
        string Name { get; }
    }
}