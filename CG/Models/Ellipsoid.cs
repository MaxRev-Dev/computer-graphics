﻿using GraphicExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Helpers;
using Playground.Projections.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Playground.Models
{
    internal class Ellipsoid : GraphicExtension
    {
        private readonly List<int[]> _edges = new List<int[]>();

        private void DrawEllipsoid(IProjectorEngine projector, Pen pen)
        {
            if (Model3D == default) Reset(projector);

            var vertexSize = 1;
            for (var i = 0; i < Model3D.GetLength(0); i++)
            {
                var (x, y) = projector.ProjectVertexToScreen(Model3D.point(i).ToArray());
                projector.Graphics.DrawEllipse(pen, x, y, vertexSize, vertexSize);
            }
            foreach (var edge in _edges)
            {
                var (i1, i2) = (edge[0], edge[1]);
                var (x1, y1) = projector.ProjectVertexToScreen(Model3D.point(i1).ToArray());
                var (x2, y2) = projector.ProjectVertexToScreen(Model3D.point(i2).ToArray());
                projector.Graphics.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        public override void Draw(IProjectorEngine projector)
        {
            DrawEllipsoid(projector, PrimaryPen);
        }

        public override void Reset(IProjectorEngine projector)
        {
            Model3D = GenerateElipsoid();
        }

        private float[,] GenerateElipsoid()
        {
            var points = new List<float[]>();
            var n = 5;

            // -pi/2 <= tet <= pi/2
            // 0 <= lam <= 2pi 
            float r1 = 1, r2 = 2, r3 = 3;

            var max = 2 * Math.PI.F();
            var sectorDegrees = Math.PI.F() / n;
            var roundOffset = (int)(max / sectorDegrees);
            float lam = 0;
            for (int i = 0, edge = 0; lam <= max; lam = sectorDegrees * ++i)
            {
                var hasNextLayer = lam + sectorDegrees <= max;
                float tet = 0;
                for (var j = 0; tet <= max; tet = sectorDegrees * ++j, edge++)
                {
                    var point = new[]
                    {
                        (float) (r1 * Math.Sin(tet) * Math.Cos(lam)), // x
                        (float) (r2 * Math.Sin(tet) * Math.Sin(lam)), // y
                        (float) (r3 * Math.Cos(tet)), // z
                        1f
                    };

                    points.Add(point);
                    if (edge == 0)
                        continue;

                    _edges.Add(new[] { edge - 1, edge });
                    if (hasNextLayer)
                    {
                        _edges.Add(new[] { edge - 1, edge + roundOffset });
                    }
                }
            }

            return points.ToArray().Convert();
        }
    }
}