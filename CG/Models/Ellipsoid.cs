using System;
using System.Collections.Generic;
using System.Drawing;
using GraphicExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Helpers;
using Playground.Projections;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class Ellipsoid : GraphicExtension
    {
        List<int[]> _edges = new List<int[]>();
        private void DrawEllipsoid(IProjectorEngine projector, Pen pen)
        {
            if (Model3D == default) Reset();

            var s = 2;
            for (var i = 0; i < Model3D.GetLength(0); i++)
            {
                var (x, y) = projector.ProjectVertexToScreen(Model3D.point(i).ToArray());
                projector.Graphics.DrawEllipse(pen, x, y, s, s);
            }
            for (var i = 0; i < _edges.Count; i++)
            {
                var (i1, i2) = (_edges[i][0], _edges[i][1]);
                var (x1, y1) = projector.ProjectVertexToScreen(Model3D.point(i1).ToArray());
                var (x2, y2) = projector.ProjectVertexToScreen(Model3D.point(i2).ToArray());
                projector.Graphics.DrawLine(pen, x1, y1, x2, y2);
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
            var edges = new List<int[]>();
            var n = 3;

            // -pi/2 <= tet <= pi/2
            // 0 <= lam <= 2pi 
            float r1 = 1, r2 = 2, r3 = 3;
            for (float lam = 0, iv = 0; lam <= 2 * Math.PI.F(); lam += Math.PI.F() / n)
            {
                var isLast = (lam + Math.PI.F() / n) >= 2 * Math.PI.F();
                var fst = iv;
                for (float tet = 0; tet <= Math.PI.F(); tet += Math.PI.F() / n, iv += 1)
                {
                    var point = new[]
                    {
                        (float) (r1 * Math.Sin(tet) * Math.Cos(lam)), // x
                        (float) (r2 * Math.Sin(tet) * Math.Sin(lam)), // y
                        (float) (r3 * Math.Cos(tet)), // z
                        1f
                    };

                    points.Add(point);

                    if (tet > 0 && !isLast)
                    {
                        edges.Add(new[] { (int)iv - 1, (int)iv });
                        edges.Add(new[] { (int)iv - 1, (int)iv + n });
                    }
                }
                if (!isLast)
                    edges.Add(new[] { (int)iv, (int)fst });
            }

            _edges = edges;
            return points.ToArray().Convert();
        }

    }
}