using System;
using Playground.Helpers;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class Hyperbola : GraphicExtension
    {
        private void Generate(IProjectorEngine projector)
        {
            var eps = 0.1;
            var b = 20f;
            var a = 6f;
            var u = .15;
            var n = 20;
            var p = (float)(a * Math.Cosh(u));
            var q = (float)(b * Math.Sinh(u));
            var c1 = a / b;
            var c2 = b / a;
            var c = (float)Math.Cosh(eps);
            var s = (float)Math.Sinh(eps);

            for (int i = 0; i < n; i++)
            {
                p = p * c + c1 * q * s;
                q = q * c + c2 * p * s; 
                projector.Graphics.DrawRectangle(PrimaryPen, p, q, 3, 3);
            }
        }


        public override void Draw(IProjectorEngine projector)
        {
            Generate(projector);
        }
    }
}