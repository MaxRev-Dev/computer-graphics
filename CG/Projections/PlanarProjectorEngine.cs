using System.Drawing;
using GraphicExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Projections.Abstractions;

namespace Playground.Projections
{
    internal class PlanarProjectorEngine : ProjectorEngine
    {
        public PlanarProjectorEngine(Graphics graphics) : base(graphics)
        {
        }


        // projection
        private float[,] worldToCamera;
        private float[,] projMatrix;

        // opengl projection properties
        private float angleOfView;
        private float near;
        private float far;
        public override void ResetWorld()
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

        public override void OnFrame()
        {
            CG.setProjectionMatrix(projMatrix, angleOfView, near, far);
        }

        public override (float x, float y) ProjectVertexToScreen(float[] vertex3d)
        {
            var vertCamera = vertex3d.Multiply(worldToCamera);
            return PointToScreen(vertCamera.Multiply(projMatrix));
        }

    }
}