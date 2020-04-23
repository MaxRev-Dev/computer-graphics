using System;
using MaxRev.Extensions.Binary;
using MaxRev.Extensions.Matrix;
using Playground.Helpers.Reflection;
using Playground.Projections.Abstractions;

namespace Playground.Projections.Engines
{
    internal enum AxonometricProjection
    {
        X, Y, Z,
        BasicIsometry,
        Custom
    }

    internal class AxonometricProjectorEngine : ProjectorEngine
    {
        public AxonometricProjectorEngine()
        {
            PHI = 30;
            PSI = 75;
        }

        [Modifiable(Scaling = 1f, Min = 15f, Max = 45)]
        public float PSI
        {
            get => (float)(_psi / Math.PI * 180);
            set => _psi = (float)(value * Math.PI / 180);
        }

        [Modifiable(Scaling = 1f, Min = 30f, Max =  90)]
        public float PHI
        {
            get => (float)(_phi / Math.PI * 180);
            set => _phi = (float)(value * Math.PI / 180);
        }

        private readonly Func<float, float, float[,]> _project = (psi, ph) => new[,]
        {
            {(float) Math.Cos(psi), (float) (-Math.Sin(psi) * Math.Sin(ph)), 0, 0},
            {0, (float) Math.Cos(ph), 0, 0},
            {-Math.Sin(psi), (float) (-Math.Cos(psi) * Math.Sin(ph)), 0, 0},
            {0, 0, 0, 1}
        }.Cast<double, float>();

        [Modifiable]
        public AxonometricProjection Projection { get; set; }

        private readonly float[,] orthoX =
        {
            {0f, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 1, 0},
            {0, 0, 0, 1},
        };

        private readonly float[,] orthoY =
        {
            {0f, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 1, 0},
            {0, 0, 0, 1},
        };

        private readonly float[,] orthoZ =
        {
            {1f, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 1},
        };

        private readonly float[,] basicIso = {
            {0.7071f, -0.408f, 0,0},
            {0, 0.816f, 0,0},
            {-0.7071f, -0.408f, 0,0},
            {0, 0, 0,1},
        };

        private float _psi ;
        private float _phi;

        protected override (float x, float y) PointToScreen(float[] point2D)
        {
            return (point2D[0] * 100 + 0.5f * DrawableWidth,
                point2D[1] * 100 + 0.5f * DrawableHeight);
        }

        public override (float x, float y) ProjectVertexToScreen(float[] vertex3d)
        {
           var  matrix = Projection switch
            {
                AxonometricProjection.X => orthoX,
                AxonometricProjection.Y => orthoY,
                AxonometricProjection.Z => orthoZ,
                AxonometricProjection.BasicIsometry => basicIso,
                AxonometricProjection.Custom => _project(PSI, PHI),
                _ => throw new ArgumentOutOfRangeException(),
            }; 
            var point = vertex3d.Multiply(matrix);
            return PointToScreen(point);
        }
    }
}