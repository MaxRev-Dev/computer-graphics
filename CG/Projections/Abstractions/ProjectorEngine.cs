using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using GraphicsExtensions;
using Playground.Helpers.Abstractions;
using static System.Double;

namespace Playground.Projections.Abstractions
{
    internal abstract class ProjectorEngine : IProjectorEngine
    {
        private GraphicContext _context;

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
            DrawLine(pen, x1, y1, x2, y2);
        }

        private void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (!(IsFinite(x1) ||
                IsFinite(y1) ||
                IsFinite(x2) ||
                IsFinite(y2)))
                return;
            Graphics.DrawLine(pen, x1, y1, x2, y2);
        }

        protected static bool IsFinite(float value)
        {
            return IsFinite((double)value);
        }

        protected static bool IsFinite(double value)
        {
            return !IsNaN(value) && !IsInfinity(value);
        }

        public void DrawLine(Pen pen, Vector3 vertex3d1, Vector3 vertex3d2)
        {
            var (x1, y1) = ProjectVertexToScreen(vertex3d1);
            var (x2, y2) = ProjectVertexToScreen(vertex3d2);
            DrawLine(pen, x1, y1, x2, y2);
        }

        public abstract (float x, float y) ProjectVertexToScreen(float[] vertex3d);

        public void FillPolygon(Brush brush, (float x, float y, float z)[] vertex3d)
        {
            var center = ProjectVertexToScreen(vertex3d.Polygon3DCenter());
            var screenPoly = vertex3d.Select(ProjectVertexToScreen).ToArray();
            using (var pen = new Pen(brush))
            {
                Graphics.DrawPolygon(pen, screenPoly.Select(x => new PointF(x.x, x.y)).ToArray());
                ExperimentalFloodFill(Bitmap, screenPoly, ((int)center.x, (int)center.y), pen.Color);
            }
        }


        private bool TryDropVertex((float x, float y, float z) projectedVert)
        {
            // out of visible screen bounds
            return projectedVert.x < -1 ||
                   projectedVert.x > 1 ||
                   projectedVert.y < -1 ||
                   projectedVert.y > 1;
        }

        private void ExperimentalFloodFill(Bitmap bmp, (float x, float y)[] poly, (int x, int y) start, Color fillColor)
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

        public float[] ViewVector(float[,] model) => model.FigureCenter().Take(3).ToArray();

        public Bitmap Bitmap => _context.Bitmap;
        public Graphics Graphics => _context.Graphics;
        public GraphicContext Context => _context;

        public IProjectorEngine Use(GraphicContext context)
        {
            _context = context;
            if (!IsReady)
                ResetWorld();
            return this;
        }

        public virtual bool IsReady { get; protected set; } = true;
    }
}