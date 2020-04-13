using System;
using System.Collections.Generic;
using System.Drawing;
using GraphicExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Helpers;

namespace Playground.Models
{
    internal class Elipsoid : GraphicExtension
    { 
        private void DrawEllipsoid(IProjectorEngine projector, Pen pen)
        {
            if (Model3D == default) Reset();

            var s = 2;
            for (var i = 0; i < Model3D.GetLength(0); i++)
            {
                var (x, y) = projector.ProjectVertexToScreen(Model3D.point(i).ToArray());
                projector.Graphics.DrawEllipse(pen, x, y, s, s);
            }
        }
         
        public override void Draw(IProjectorEngine projector)
        {
            DrawEllipsoid(projector, PrimaryPen);
        }


        public override void Reset()
        {
            Model3D = GenerateElipsoid();
        }

        private float[,] GenerateElipsoid()
        {
            var points = new List<float[]>();

            var n = 30;

            // -pi/2 <= tet <= pi/2
            // 0 <= lam <= 2pi 
            float r1 = 1, r2 = 2, r3 = 3;
            for (float lam = 0; lam <= 2 * Math.PI.F(); lam += Math.PI.F() / n)
            {
                for (float tet = 0; tet <= Math.PI.F(); tet += Math.PI.F() / n)
                {
                    points.Add(new[]
                    {
                        (float) (r1 * Math.Sin(tet) * Math.Cos(lam)), // x
                        (float) (r2 * Math.Sin(tet) * Math.Sin(lam)), // y
                        (float) (r3 * Math.Cos(tet)), // z
                        1f
                    });
                }
            }

            return points.ToArray().Convert();
        }

    }
}