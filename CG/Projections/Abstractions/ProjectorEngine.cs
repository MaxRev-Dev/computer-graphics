using System.Drawing;
using GraphicExtensions;

namespace Playground.Projections.Abstractions
{
    internal abstract class ProjectorEngine : IProjectorEngine
    {
        public ProjectorEngine(Graphics graphics)
        {
            Graphics = graphics;
        }

        public float DrawableWidth => Graphics.VisibleClipBounds.Width;
        public float DrawableHeight => Graphics.VisibleClipBounds.Height;

        protected virtual (float x, float y) PointToScreen(float[] point2D)
        {
            return ((point2D[0] + 1) * 0.5f * DrawableWidth,
                (point2D[1] + 1) * 0.5f * DrawableHeight);
        } 

        public (float x, float y) ProjectVertexToScreen((float x, float y, float z) vertex3d)
        {
            return ProjectVertexToScreen(vertex3d.ToArray());
        }

        public abstract (float x, float y) ProjectVertexToScreen(float[] vertex3d);

        public virtual void ResetWorld() { }

        public virtual void OnFrame() { }

        public float[] ViewVector => new[] { 0f, -1, 0 };

        public Graphics Graphics { get; private set; }

        public void Use(Graphics graphics)
        {
            Graphics = graphics;
        }
    }
}