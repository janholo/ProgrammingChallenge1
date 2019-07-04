using System;
using System.Numerics;

namespace ConsolePong
{
    public static class Helpers
    {
        public static Vector2 RotatedUnitVector(float angleInRadians)
        {
            return new Vector2((float)Math.Cos(angleInRadians), (float)Math.Sin(angleInRadians));
        }
    }
}