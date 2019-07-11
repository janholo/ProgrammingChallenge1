using System.Numerics;

namespace ConsolePong.GameLogic
{
    internal class Ball
    {
        public Ball(Vector2 position, Vector2 velocityInUnitsPerSecond)
        {
            Position = position;
            VelocityInUnitsPerSecond = velocityInUnitsPerSecond;
        }

        public Vector2 Position { get; }

        public Vector2 VelocityInUnitsPerSecond { get; }
    }
}