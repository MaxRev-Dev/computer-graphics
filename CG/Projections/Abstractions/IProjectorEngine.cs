using System.Drawing;
using System.Numerics;

namespace Playground.Projections.Abstractions
{
    internal interface IProjectorEngine
    {
        (float x, float y) ProjectVertexToScreen(float[] vertex3d);
        (float x, float y) ProjectVertexToScreen(Vector3 vertex3d);
        (float x, float y) ProjectVertexToScreen((float x, float y, float z) vertex3d);
        void ResetWorld();
        void OnFrame();
        float[] ViewVector { get; }
        Graphics Graphics { get; }
        void Use(Graphics graphics);
    }
}