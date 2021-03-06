﻿using System.Drawing;
using System.Drawing.Text;
using GraphicsExtensions;
using Playground.Helpers.Abstractions;
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
                projector.DrawLine(colors[i - 1], Model3D.point(i), Model3D.point(0));
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