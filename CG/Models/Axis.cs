using System.Drawing;
using System.Drawing.Text;
using GraphicExtensions;
using Playground.Helpers;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal sealed class Axis : GraphicExtension
    {
        public Axis()
        {
            Global = true;
        }

        public override void Reset(IProjectorEngine projector)
        {
            Model3D = new[,]
            {
                {0f, 0, 0, 1},
                {1f, 0, 0, 1},
                {0, -1, 0, 1},
                {0, 0, 1, 1}
            };
            base.Reset(projector);
        }

        public override void Draw(IProjectorEngine projector)
        {
            if (Model3D == default) Reset(projector);

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

            GrawAxisName(projector, "x", Model3D.point(1));
            GrawAxisName(projector, "y", Model3D.point(2));
            GrawAxisName(projector, "z", Model3D.point(3));
        }

        private void GrawAxisName(IProjectorEngine projector, string name, (float x, float y, float z) vertex)
        {
            var vtos = projector.ProjectVertexToScreen(vertex);
            projector.Graphics.DrawString(name, new Font(new FontFamily(GenericFontFamilies.Monospace), 10),
                Brushes.Black, vtos.x, vtos.y);
        }
    }
}