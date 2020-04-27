using System;

namespace Playground.Helpers.Reflection
{
    public class ModifiableAttribute : Attribute
    {
        public float Scaling { get; set; } = 1;

        public float Min { get; set; }

        public float Max { get; set; } = 100;

        public float Step { get; set; } = 1;

        public bool RequiresReset { get; set; } = true;

        public float GetSimpleValue(int v)
        {
            return Math.Min(v * Scaling * Step, Max);
        }

        public int GetIntegerForControl(float val)
        {
            return (int) (Math.Min((Min + Max) / 2,  val) * (1f / Scaling) * Step);
        }
    }
}