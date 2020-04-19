using System.Drawing;
using System.Numerics;

namespace Playground.Projections.Abstractions
{
    internal interface IProjectorEngine
    {
        (float x, float y) ProjectVertexToScreen(float[] vertex3d);
        (float x, float y) ProjectVertexToScreen(Vector3 vertex3d);
        (float x, float y) ProjectVertexToScreen((float x, float y, float z) vertex3d);
        void DrawLine(Pen pen, (float x, float y, float z) vertex3d1, (float x, float y, float z) vertex3d2);
        void DrawLine(Pen pen, Vector3 vertex3d1, Vector3 vertex3d2);
        void FillPolygon(Brush brush, (float x, float y, float z)[] vertex3d);
        void ResetWorld();
        void OnFrame();
        float[] ViewVector { get; }
        Graphics Graphics { get; }
        void Use(Graphics graphics, Bitmap bitmap);
    }
}