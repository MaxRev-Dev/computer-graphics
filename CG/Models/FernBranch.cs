using System;
using Playground.Helpers;
using Playground.Projections;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class FernBranch : GraphicExtension
    {
        public float Alfa { get; set; } = (float)(4 * Math.PI / 180);
        public float Beta { get; set; } = (float)(74 * Math.PI / 180);
        public float K0 { get; set; } = 0.14f;
        public float K1 { get; set; } = 0.42f;
        public float Lmin { get; set; } = 10;

        private void Generate(IProjectorEngine projector, float x1, float y1, float x2, float y2, int iter)
        {
            if (iter == 0 || !(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) > Lmin))
                return;
            /*
             alternative via vectors
             var p1 = new Vector(x1, y1);
                 var p2 = new Vector(x2, y2);
                 var first = Vector.Subtract(p2, p1);
                 var p3 = first.Rotate(Alfa);*/
            var x3 = (x2 - x1) * Math.Cos(Alfa) - (y2 - y1) * Math.Sin(Alfa) + x1;
            var y3 = (x2 - x1) * Math.Sin(Alfa) + (y2 - y1) * Math.Cos(Alfa) + y1;
            var x4 = x1 * (1 - K0) + x3 * K0;
            var y4 = y1 * (1 - K0) + y3 * K0;
            var x5 = x4 * (1 - K1) + x3 * K1;
            var y5 = y4 * (1 - K1) + y3 * K1;
            var x6 = (x5 - x4) * Math.Cos(Beta) - (y5 - y4) * Math.Sin(Beta) + x4;
            var y6 = (x5 - x4) * Math.Sin(Beta) + (y5 - y4) * Math.Cos(Beta) + y4;
            var x7 = (x5 - x4) * Math.Cos(Beta) + (y5 - y4) * Math.Sin(Beta) + x4;
            var y7 = -(x5 - x4) * Math.Sin(Beta) - (y5 - y4) * Math.Cos(Beta) + y4;
            if (Math.Abs(x1 - x4) > 1E-9 && Math.Abs(y1 - y4) > 1E-9)
            {
                projector.Graphics.DrawLine(PrimaryPen, x1, y1, (float) x4, (float) y4);
                Generate(projector, (float) x4, (float) y4, (float) x3, (float) y3, iter);
                Generate(projector, (float) x4, (float) y4, (float) x6, (float) y6, iter - 1);
                Generate(projector, (float) x4, (float) y4, (float) x7, (float) y7, iter - 1);
            }
        }

        public override void Draw(IProjectorEngine projector)
        {
            var b = projector.Graphics.VisibleClipBounds;
            Generate(projector,
                b.Width / 2,
                b.Height - 200,  // center and a bit up
                b.Width / 2,
                10,10);
        }
    }
}