using System;

namespace Playground.Helpers
{
    public class ModifiableAttribute : Attribute
    {
        public float Scaling { get; set; } = 1;

        public float Min { get; set; }

        public float Max { get; set; } = 100;

        public bool RequiresReset { get; set; } = true;
    }
}