using System;
using Playground.Helpers.Abstractions;
using Playground.Helpers.Reflection;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class FernBranch : GraphicExtension
    {
        public FernBranch()
        {
            AlfaAngle = 5;
            BetaAngle = 95;
        }

        private float _alfa;
        private float _beta;

        [Modifiable(Scaling = 1f, Min = 0.01f, Step = .5f, Max = 180)]
        public float BetaAngle
        {
            get => _beta.ToDeg();
            set => _beta = value.FromDeg();
        }

        [Modifiable(Scaling = 1f, Min = 0.01f, Step = .5f, Max = 180)]
        public float AlfaAngle
        {
            get => _alfa.ToDeg();
            set => _alfa = value.FromDeg();
        }

        [Modifiable(Scaling = .1f, Min = 0.01f, Max = 10)]
        public float K0 { get; set; } = 0.14f;
        [Modifiable(Scaling = .1f, Min = 0.01f, Max = 10)]
        public float K1 { get; set; } = 0.42f;
        [Modifiable(Scaling = 1f, Min = 1, Max = 100)]
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
            var x3 = (x2 - x1) * Math.Cos(_alfa) - (y2 - y1) * Math.Sin(_alfa) + x1;
            var y3 = (x2 - x1) * Math.Sin(_alfa) + (y2 - y1) * Math.Cos(_alfa) + y1;
            var x4 = x1 * (1 - K0) + x3 * K0;
            var y4 = y1 * (1 - K0) + y3 * K0;
            var x5 = x4 * (1 - K1) + x3 * K1;
            var y5 = y4 * (1 - K1) + y3 * K1;
            var x6 = (x5 - x4) * Math.Cos(_beta) - (y5 - y4) * Math.Sin(_beta) + x4;
            var y6 = (x5 - x4) * Math.Sin(_beta) + (y5 - y4) * Math.Cos(_beta) + y4;
            var x7 = (x5 - x4) * Math.Cos(_beta) + (y5 - y4) * Math.Sin(_beta) + x4;
            var y7 = -(x5 - x4) * Math.Sin(_beta) - (y5 - y4) * Math.Cos(_beta) + y4;
            if (Math.Abs(x1 - x4) > 1E-9 && Math.Abs(y1 - y4) > 1E-9)
            {
                projector.Graphics.DrawLine(PrimaryPen, x1, y1, (float)x4, (float)y4);
                Generate(projector, (float)x4, (float)y4, (float)x3, (float)y3, iter);
                Generate(projector, (float)x4, (float)y4, (float)x6, (float)y6, iter - 1);
                Generate(projector, (float)x4, (float)y4, (float)x7, (float)y7, iter - 1);
            }
        }

        public override void Draw(IProjectorEngine projector)
        {
            var b = projector.Context;
            Generate(projector,
                b.DrawableWidth / 2,
                b.DrawableHeight - 200,  // center and a bit up
                b.DrawableWidth / 2,
                10, 10);
        }
    }
}