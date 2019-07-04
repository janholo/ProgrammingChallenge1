using System.Numerics;

namespace ConsolePong
{
    internal class Ball
    {
        public Ball(Vector2 positon, Vector2 velocityInUnitsPerSecond)
        {
            Position = positon;
            VelocityInUnitsPerSecond = velocityInUnitsPerSecond;
        }

        public Vector2 Position { get; }

        public Vector2 VelocityInUnitsPerSecond { get; }
    }
}