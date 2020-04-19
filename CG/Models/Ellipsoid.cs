using GraphicExtensions;
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
            try
            {

                var vertexSize = 1;
                for (var i = 0; i < Model3D.GetLength(0); i++)
                {
                    var (x, y) = projector.ProjectVertexToScreen(Model3D.point(i).ToArray());
                    projector.Graphics.DrawEllipse(pen, x, y, vertexSize, vertexSize);
                }
                foreach (var edge in _edges)
                { 
                    projector.DrawLine(pen, Model3D.point(edge[0]), Model3D.point(edge[1]));
                }
            }
            catch (IndexOutOfRangeException)
            {

            }
        }

        public override void Draw(IProjectorEngine projector)
        {
            DrawEllipsoid(projector, PrimaryPen);
        }

        public override void Reset(IProjectorEngine projector)
        {
            Model3D = GenerateElipsoid();
            base.Reset(projector);
        }

        private float[,] GenerateElipsoid()
        {
            var points = new List<float[]>();
            _edges.Clear();
            // -pi/2 <= tet <= pi/2
            // 0 <= lam <= 2pi  

            var max = 2 * Math.PI.F();
            var sectorDegrees = Math.PI.F() / N;
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
                        (float) (R1 * Math.Sin(tet) * Math.Cos(lam)), // x
                        (float) (R2 * Math.Sin(tet) * Math.Sin(lam)), // y
                        (float) (R3 * Math.Cos(tet)), // z
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

        [Modifiable(Scaling = 1, Max = 100, Min = 3)]
        public int N { get; set; } = 20;

        [Modifiable(Scaling = 1f, Min = 0, Max = 10)]
        public float R3 { get; set; } = 3;

        [Modifiable(Scaling = 1f, Min = 0, Max = 10)]
        public float R2 { get; set; } = 2;

        [Modifiable(Scaling = 1f, Min = 0, Max = 10)]
        public float R1 { get; set; } = 1;
    }
}