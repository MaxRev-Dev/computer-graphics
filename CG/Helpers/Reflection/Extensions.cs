using System;
using System.Windows;

namespace Playground.Helpers.Reflection
{
    public static class Extensions
    {
        public static float[] ToArray3D(this Vector v, float z = 0)
        {
            return new[] { (float)v.X, (float)v.Y, z, 1 };
        }

        public static Vector ToVector2D(this float[] v)
        {
            return new Vector(v[0], v[1]);
        }

        public static float FromDeg(this float val)
        {
            return (float)(val * Math.PI / 180);
        }

        public static float ToDeg(this float val)
        {
            return (float) (val / Math.PI * 180);
        }
    }
}