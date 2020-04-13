using System.Drawing;
using GraphicExtensions;
using Playground.Helpers;

namespace Playground.Models
{
    internal class Axis : GraphicExtension
    {
        public override void Reset()
        {
            Model3D = new[,]
            {
                {0f, 0, 0, 1},
                {1f, 0, 0, 1},
                {0, 1, 0, 1},
                {0, 0, 1, 1}
            };
        }
        public override void Draw(IProjectorEngine projector)
        {
            if (Model3D == default) Reset();

            var colors = new[]
            {
                Pens.Red,
                Pens.Green,
                Pens.Yellow
            };
            for (var i = 1; i < Model3D.GetLength(0); i++)
            {
                var (x1, y1) = projector.ProjectVertexToScreen(Model3D.point(i));
                var (x2, y2) = projector.ProjectVertexToScreen(Model3D.point(0));
                projector.Graphics.DrawLine(colors[i - 1], new PointF(x1, y1), new PointF(x2, y2));
            }
        }
    }
}