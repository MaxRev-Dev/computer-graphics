using System;
using System.Drawing;
using Playground.Helpers.Abstractions;
using Playground.Helpers.Reflection;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class NewtonBasins : GraphicExtension
    {
        struct Point
        {
            public double x;
            public double y;
        }
        [Modifiable(Max = 100, Min = 1, Scaling = 1)]
        public int Iterations { get; set; } = 25;
        public double Min { get; set; } = 1e-8;
        public double Max { get; set; } = 1e+8;

        public void FillNewtonBasins(Graphics g, int mx1, int my1)
        {
            var mx = mx1 / 4;
            var my = my1 / 4;
            var offsetX = mx1 / 4;
            var offsetY = my1 / 4;
            using (var pen = new Pen(Color.White))
            {
                for (int y = -my; y < my; y++)
                {
                    for (int x = -mx; x < mx; x++)
                    {
                        var n = 0;
                        Point z;
                        z.x = x * 0.005;
                        z.y = y * 0.005;
                        var d = z;

                        while (Math.Pow(z.x, 2) + Math.Pow(z.y, 2) < Max &&
                               Math.Pow(d.x, 2) + Math.Pow(d.y, 2) > Min &&
                               n < Iterations)
                        {
                            var t = z;
                            var p = Math.Pow(Math.Pow(t.x, 2) + Math.Pow(t.y, 2), 2);
                            z.x = 2f / 3 * t.x + (Math.Pow(t.x, 2) - Math.Pow(t.y, 2)) / (3 * p);
                            z.y = 2d / 3 * t.y * (1 - t.x / p);
                            d.x = Math.Abs(t.x - z.x);
                            d.y = Math.Abs(t.y - z.y);
                            n++;
                        }

                        pen.Color = Color.FromArgb(255, (n * 12) % 255, 0, (n * 12) % 255);
                        g.DrawRectangle(pen, mx + x + offsetX, my + y + offsetY, 1, 1);
                    }
                }
            }
        }


        public override void Draw(IProjectorEngine projector)
        {
            var graphics = projector.Graphics;
            FillNewtonBasins(graphics,
                (int)graphics.VisibleClipBounds.Width,
                (int)graphics.VisibleClipBounds.Height);
        }

    }
}