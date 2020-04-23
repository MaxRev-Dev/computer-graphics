using GraphicsExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Projections.Abstractions;

namespace Playground.Projections.Engines
{
    internal class PointProjectorEngine : ProjectorEngine
    { 
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
            near = 1f;
            far = 1000;
            projMatrix = MatrixExtensions.IdentityF(4);
            worldToCamera[3, 1] = -10;
            worldToCamera[3, 2] = -20;
            // CG.setProjectionMatrix(projMatrix, angleOfView, near, far);
        }

        public override bool IsReady => projMatrix != default;

        public override void OnFrame()
        {
            CG.setProjectionMatrix(projMatrix, angleOfView, near, far);
        }

        public override (float x, float y) ProjectVertexToScreen(float[] vertex3d)
        {
            var vertCamera = vertex3d.MultiplyNorm(worldToCamera);
            return PointToScreen(vertCamera.MultiplyNorm(projMatrix));
        }

    }
}