using GraphicExtensions;
using MaxRev.Extensions.Binary;
using MaxRev.Extensions.Matrix;
using Playground.Projections.Abstractions;

namespace Playground.Projections.Engines
{
    internal class PerspectiveProjectorEngine : ProjectorEngine
    {
        // projection
        private float[,] worldToCamera;
        private float[,] projMatrix;
          
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
            base.ResetWorld();
        }

        public override bool IsReady => projMatrix != default;

        // opengl projection properties
        private float angleOfView;
        private float near;
        private float far;
        public override void OnFrame()
        {
            float imageAspectRatio = DrawableWidth / DrawableWidth;
            float b, t, l, r;
            CG.gluPerspective(angleOfView, imageAspectRatio, near, out b, out t, out l, out r);
            CG.glFrustum(b, t, l, r, near, far, projMatrix);
            base.OnFrame();
        }

        public override (float x, float y) ProjectVertexToScreen(float[] vertex3d)
        {
            var vertCamera = vertex3d.MultiplyNorm(worldToCamera);
            return PointToScreen(vertCamera.MultiplyNorm(projMatrix));
        }

        public void TransformCamera(float[,] trs)
        {
            var cameraToWord = worldToCamera.Cast<float, double>().Inverse();
            var res = cameraToWord.Cast<double, float>().Multiply(trs);
            worldToCamera = res.Cast<float, double>().Inverse().Cast<double, float>();

        }
    }
}