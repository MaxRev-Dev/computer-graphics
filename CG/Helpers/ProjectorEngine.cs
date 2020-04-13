using System.Drawing;
using GraphicExtensions;
using MaxRev.Extensions.Matrix;

namespace Playground.Helpers
{
    internal class ProjectorEngine : IProjectorEngine
    {
        private Graphics _graphics;

        public ProjectorEngine(Graphics graphics)
        {
            _graphics = graphics;
        }

        private float DrawableWidth => _graphics.VisibleClipBounds.Width;
        private float DrawableHeight => _graphics.VisibleClipBounds.Height;

        // projection
        private float[,] worldToCamera;
        private float[,] projMatrix;

        // opengl projection properties
        private float angleOfView;
        private float near;
        private float far;
        public float[] ViewVector => new[] { 0f, -1, 0 };
        public Graphics Graphics => _graphics;
        public void Use(Graphics graphics)
        {
            _graphics = graphics;
        }

        private void ResetWorld()
        {
            // mainFigure = CG.ApplyTransform(mainFigure, CG.TranslateZ(-10));
            worldToCamera = MatrixExtensions.IdentityF(4);
            angleOfView = 90;
            near = 0.1f;
            far = 100;
            projMatrix = MatrixExtensions.IdentityF(4);
            worldToCamera[3, 1] = -10;
            worldToCamera[3, 2] = -20;
            // CG.setProjectionMatrix(projMatrix, angleOfView, near, far);
        }

        public void OnFrame()
        {
            GraphicExtensions.CG.setProjectionMatrix(projMatrix, angleOfView, near, far);
        }

        public (float x, float y) ProjectVertexToScreen(float[] vertex3d)
        {
            var vertCamera = vertex3d.Multiply(worldToCamera);
            return PointToScreen(vertCamera.Multiply(projMatrix).ToPoint2D());
        }

        private (float x, float y) PointToScreen((float x, float y) point2D)
        {
            return ((point2D.x + 1) * 0.5f * DrawableWidth,
                (point2D.y + 1) * 0.5f * DrawableHeight);

        }

        public (float x, float y) ProjectVertexToScreen((float x, float y, float z) vertex3d)
        {
            return ProjectVertexToScreen(vertex3d.ToArray());
        }

        void IProjectorEngine.ResetWorld()
        {
            ResetWorld();
        }
    }
}