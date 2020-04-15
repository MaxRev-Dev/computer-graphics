using System;
using System.Drawing;
using GraphicExtensions;
using MaxRev.Extensions.Binary;
using Playground.Projections.Abstractions;

namespace Playground.Projections
{
    internal class DimetricProjectorEngine : ProjectorEngine
    {
        public DimetricProjectorEngine(Graphics graphics) : base(graphics)
        {
        }

        public float PSI { get; set; } = (float)(45f * Math.PI / 180);
        public float PHI { get; set; } = (float)(45f * Math.PI / 180);

        private readonly Func<float, float, float[,]> _project = (psi, ph) => new[,]
        {
            {(float) Math.Cos(psi), (float) (-Math.Sin(psi) * Math.Sin(ph)), 0, 0},
            {0, (float) Math.Cos(ph), 0, 0},
            {-Math.Sin(psi), (float) (-Math.Cos(psi) * Math.Sin(ph)), 0, 0},
            {0, 0, 0, 1}
        }.Cast<double, float>();

        private readonly float[,] basicOrtho = {
            {1f, 0, 0},
            {0, 1, 0},
            {0, 0, 0},
        };
        private readonly float[,] basicIso = {
            {0.7071f, -0.408f, 0,0},
            {0, 0.816f, 0,0},
            {-0.7071f, -0.408f, 0,0},
            {0, 0, 0,1},
        };
         
        public override (float x, float y) ProjectVertexToScreen(float[] vertex3d)
        {
            var point = vertex3d.Multiply(_project(PSI, PHI));
            return PointToScreen(point);

        }
    }
}