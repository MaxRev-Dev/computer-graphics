using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using GraphicExtensions;

namespace Playground.Projections.Abstractions
{
    internal abstract class ProjectorEngine : IProjectorEngine
    {
        public ProjectorEngine(Bitmap bitmap, Graphics graphics)
        {
            Bitmap = bitmap;
            Graphics = graphics;
        }

        public float DrawableWidth => Graphics.VisibleClipBounds.Width;
        public float DrawableHeight => Graphics.VisibleClipBounds.Height;

        protected virtual (float x, float y) PointToScreen(float[] point2D)
        {
            return ((point2D[0] + 1) * 0.5f * DrawableWidth,
                (point2D[1] + 1) * 0.5f * DrawableHeight);
        }

        public (float x, float y) ProjectVertexToScreen(Vector3 vertex3d)
        {
            return ProjectVertexToScreen(new[] { vertex3d.X, vertex3d.Y, vertex3d.Z });
        }

        public (float x, float y) ProjectVertexToScreen((float x, float y, float z) vertex3d)
        {
            return ProjectVertexToScreen(vertex3d.ToArray());
        }

        public void DrawLine(Pen pen, (float x, float y, float z) vertex3d1, (float x, float y, float z) vertex3d2)
        {
            var (x1, y1) = ProjectVertexToScreen(vertex3d1);
            var (x2, y2) = ProjectVertexToScreen(vertex3d2);
            Graphics.DrawLine(pen, x1, y1, x2, y2);
        }

        public void DrawLine(Pen pen, Vector3 vertex3d1, Vector3 vertex3d2)
        {
            var (x1, y1) = ProjectVertexToScreen(vertex3d1);
            var (x2, y2) = ProjectVertexToScreen(vertex3d2);
            Graphics.DrawLine(pen, x1, y1, x2, y2);
        }

        public abstract (float x, float y) ProjectVertexToScreen(float[] vertex3d);

        public void FillPolygon(Brush brush, (float x, float y, float z)[] vertex3d)
        {
            var center = ProjectVertexToScreen(vertex3d.Polygon3DCenter());
            var screenPoly = vertex3d.Select(ProjectVertexToScreen).ToArray();
            using (var pen = new Pen(brush))
            {
                Graphics.DrawPolygon(pen, screenPoly.Select(x => new PointF(x.x, x.y)).ToArray());
                FloodFill(Bitmap, screenPoly, ((int)center.x, (int)center.y), pen.Color);
            }
        }

        private void FloodFill(Bitmap bmp, (float x, float y)[] poly, (int x, int y) start, Color fillColor)
        {
            if (!poly.ContainsPoint(start))
                return;

            // fill the background
            var background = bmp.GetPixel(start.x, start.y);
            if (background.ToArgb().Equals(fillColor.ToArgb()))
            {
                return;
            }

            var pixels = new Queue<(int X, int Y)>();
            int dir = 0;
            while (dir++ < 2) // overpass recursion (upY=2,downY=1)
            {
                /* move up on 2nd pass */
                pixels.Enqueue(dir == 2 ? (start.x, start.y - 1) : start);
                while (pixels.Count != 0)
                {
                    var temp = pixels.Dequeue();
                    if (!poly.ContainsPoint((temp.X, temp.Y)))
                    {
                        continue;
                    }

                    // move up to the top of poly
                    var currentLayer = temp.Y;
                    var canMoveLeft = false;
                    var canMoveRight = false;
                    while (currentLayer < bmp.Height &&
                           currentLayer >= 0 &&
                           // check if we are on the bounds pixel
                           bmp.GetPixel(temp.X, currentLayer) == background)
                    {
                        // set current
                        bmp.SetPixel(temp.X, currentLayer, fillColor);

                        // move left
                        if (!canMoveLeft && temp.X > 0 &&
                            bmp.GetPixel(temp.X - 1, currentLayer) == background)
                        {
                            pixels.Enqueue((temp.X - 1, currentLayer));
                            canMoveLeft = true;
                        }
                        else if (canMoveLeft && temp.X - 1 == 0 &&
                                 bmp.GetPixel(temp.X - 1, currentLayer) != background)
                        {
                            canMoveLeft = false;
                        }

                        // move right
                        if (!canMoveRight && temp.X < bmp.Width - 1 &&
                            bmp.GetPixel(temp.X + 1, currentLayer) == background)
                        {
                            pixels.Enqueue((temp.X + 1, currentLayer));
                            canMoveRight = true;
                        }
                        else if (canMoveRight && temp.X < bmp.Width - 1 &&
                                 bmp.GetPixel(temp.X + 1, currentLayer) != background)
                        {
                            canMoveRight = false;
                        }

                        // move down or up on bitmap
                        if (dir == 1) currentLayer++;
                        else currentLayer--;
                    }
                }
            }
        }

        public virtual void ResetWorld() { }

        public virtual void OnFrame() { }

        public float[] ViewVector => new[] { 0f, -1, 0 };

        public Bitmap Bitmap { get; private set; }
        public Graphics Graphics { get; private set; }

        public void Use(Graphics graphics, Bitmap bitmap)
        {
            Graphics = graphics;
            Bitmap = bitmap;
        }
    }
}