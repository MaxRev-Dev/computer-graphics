using System.Collections.Generic;
using System.Linq;
using GraphicExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Helpers;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal sealed class Tetrahedron : GraphicExtension
    {
        // model  faces
        private int[][] _faces;

        public Tetrahedron()
        {
        }

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
        }
        public override void Draw(IProjectorEngine projector)
        {
            if (Model3D == default) Reset(projector);

            for (var faceIndex = 0; faceIndex < _faces.Length; faceIndex++)
            {
                var path = new List<(float x, float y)>();
                var face = GetFace(faceIndex).ToArray();
                foreach (var edge in face)
                {
                    var projectedVert = projector.ProjectVertexToScreen(edge);
                    path.Add(projectedVert);
                }

                var isVisible = IsVisibleFace(projector.ViewVector, face);

                for (var i = 0; i < path.Count; i++)
                    for (var j = i + 1; j < path.Count; j++)
                        projector.Graphics.DrawLine(isVisible ? PrimaryPen : SecondaryPen, path[i].x, path[i].y,
                            path[j].x, path[j].y);
                /*var points = path.Select(x => new PointF(x.x, x.y)).ToArray();
                _graphics.FillPolygon(isVisible ? Brushes.PaleGreen : Brushes.Aquamarine, points);*/
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