using System;
using Playground.Helpers.Abstractions;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class Hyperbola : GraphicExtension
    {
        private void Generate(IProjectorEngine projector)
        {
            var eps = 0.262;
            var b = 20f;
            var a = 20f;
            var u = .0001;
            var n = 10;
            var p = (float)(a * Math.Cosh(u));
            var q = (float)(b * Math.Sinh(u));
            var c1 = a / b;
            var c2 = b / a;
            var c = (float)Math.Cosh(eps);
            var s = (float)Math.Sinh(eps);
            (float, float) current = (0,0);
            for (int i = 0; i < n; i++)
            {
                p = p * c + c1 * q * s;
                q = q * c + c2 * p * s;
                if (i == 0)
                {
                    current = (p, q);
                    continue;
                }
                projector.Graphics.DrawLine(PrimaryPen, p, q, current.Item1, current.Item2);
                current = (p, q);
            }
        }


        public override void Draw(IProjectorEngine projector)
        {
            Generate(projector);
        }
    }
}