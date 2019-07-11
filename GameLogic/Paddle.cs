using System.Numerics;

namespace ConsolePong.GameLogic
{
    internal class Paddle
    {
        public Paddle(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; }
    }
}