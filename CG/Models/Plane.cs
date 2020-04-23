using System.Drawing;
using GraphicsExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Helpers.Abstractions;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class Plane : GraphicExtension
    {
        public override float[,] Model3D { get; protected set; }

        public override void Reset(IProjectorEngine projector)
        {
            Model3D = new[,]
            {
                {0f, 0, 0, 1},
                {0f, 2, 0, 1},
                {2, 2, 0, 1},
                {2, 0, 0, 1},
            };
            for (int i = 2; i < 3; i++)
            {
                Model3D = Merge(Model3D, Model3D.Multiply(2));
            }

            base.Reset(projector);
        }

        private float[,] Merge(float[,] m1, float[,] m2)
        {
            var l = m1.GetLength(0);
            var m3 = new float[l + l, m1.GetLength(1)];
            for (int i = 0; i < m1.GetLength(0); i++)
            {
                for (int j = 0; j < m1.GetLength(1); j++)
                {
                    m3[i, j] = m1[i, j];
                }
            }
            for (int i = 0; i < m2.GetLength(0); i++)
            {
                for (int j = 0; j < m2.GetLength(1); j++)
                {
                    m3[l + i, j] = m2[i, j];
                }
            }

            for (int i = 0; i < m3.GetLength(0); i++)
            {
                m3[i, 3] = 1;
            }

            return m3;

        }


        public override void Draw(IProjectorEngine projector)
        {
            for (var i = 0; i < Model3D.GetLength(0) - 1; i++)
            {
                projector.DrawLine(Pens.CadetBlue, Model3D.point(i), Model3D.point(i + 1));
            }

            projector.DrawLine(Pens.CadetBlue, Model3D.point(Model3D.GetLength(0) - 1), Model3D.point(0));

        }
    }
}