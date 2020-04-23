using MaxRev.Extensions.Matrix;
using Playground.Projections.Abstractions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GraphicsExtensions;
using Playground.Helpers.Abstractions;
using Playground.Helpers.Reflection;

namespace Playground.Models
{
    internal sealed class Tetrahedron : GraphicExtension
    {
        // model  faces
        private int[][] _faces;

        public override void Reset(IProjectorEngine projector)
        {
            Model3D = new[,]
            {
                {0f, -1, 0, 1}, //0 A 
                {-1f, 0, 0, 1}, //1 B
                {0f, 1, 0, 1}, //2 C
                {1f, 0, 0, 1}, //3 D
                {0f, 0, 1, 1}, //4 E
            };
            _faces = new[]
            {
                new[] {0, 1, 2, 3}, // ABCD
                new[] {1, 4, 0}, // BEA
                new[] {0, 4, 3}, // AED
                new[] {2, 4, 1}, // CEB
                new[] {3, 4, 2} // DEC
            };
            base.Reset(projector);
        }

        [Modifiable(RequiresReset = false)]
        public bool TryFill { get; set; }
        [Modifiable(RequiresReset = false)]
        public bool NativeFill { get; set; }

        public override void Draw(IProjectorEngine projector)
        {
            if (Model3D == default) Reset(projector);

            var vv = projector.ViewVector(Model3D);
            var order = new List<(bool isVisible, (float x, float y, float z)[] Location)>();
            for (var faceIndex = 0; faceIndex < _faces.Length; faceIndex++)
            {
                var face = GetFace(faceIndex).ToArray();

                var isVisible = IsVisibleFace(vv, face);

                order.Add((isVisible, face));
            }

            foreach (var face in order.OrderBy(x => x.isVisible))
            { 
                var points = face.Location.Select(projector.ProjectVertexToScreen).ToPointF();
                projector.Graphics.DrawPolygon(PrimaryPen, points);
            }
            foreach (var face in order.OrderBy(x => x.isVisible))
            {
                var points = face.Location.Select(projector.ProjectVertexToScreen).ToPointF();
                // trying to fill it with flood fill 
                if (TryFill)
                    projector.FillPolygon(!face.isVisible ? Brushes.ForestGreen : Brushes.PaleGreen, face.Location);
                if (NativeFill)
                    projector.Graphics.FillPolygon(!face.isVisible ? Brushes.ForestGreen : Brushes.PaleGreen, points);
            }
        }

        private bool IsVisibleFace(float[] viewVector, (float x, float y, float z)[] edge)
        {
            /*
             * [x1   x2 x3]
             * [y1   y2 y3]
             * [z1   z2 z3] 
             */
            // transposed matrix
            var v1 = edge[0].ToArray(); // edge1
            var v2 = edge[1].ToArray(); // edge2
            var v3 = edge[2].ToArray(); // edge3
            var a = v1.Subtract(v2);
            var b = v1.Subtract(v3);
            var n = a.Cross(b).Normalize();
            return viewVector.Dot(n) > 0;
        }

        private IEnumerable<(float x, float y, float z)> GetFace(int faceIndex)
        {
            for (var i = 0; i < _faces[faceIndex].Length; i++)
            {
                yield return Model3D.point(_faces[faceIndex][i]);
            }
        }
    }
}