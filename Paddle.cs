using System.Numerics;

namespace ConsolePong
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