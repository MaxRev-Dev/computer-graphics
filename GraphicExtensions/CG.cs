using System;
using System.Linq;
using MaxRev.Extensions.Matrix;

namespace GraphicExtensions
{
    public static class CG
    {
        public static float eps(float x) => (float)(x > 0 ? Math.PI / 300 : -Math.PI / 300);

        public static float Eps(float x) => (float)(x * Math.PI / 300);

        public static void ApplyTransform(ref float[,] m, float[,] tr)
        {
            m = tr.Transpose().Multiply(m);
        }


        public static float[] Add(float[] a, float[] b)
        {
            var ret = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                ret[i] = a[i] + b[i];
            }
            return ret;
        }

        public static float[] Cross(this float[] v1, float[] v2)
        {
            return new[]
            {
                v1[1] * v2[2] - v1[2] * v2[1],
                -v1[0] * v2[2] + v1[2] * v2[0],
                v1[0] * v2[1] - v1[1] * v2[0],
            };
        }

        public static float[,] RotX(float x) => new[,]
        {
            {1, 0, 0, 0},
            {0, (float) Math.Cos(x), (float) Math.Sin(x), 0},
            {0, (float) -Math.Sin(x), (float) Math.Cos(x), 0},
            {0, 0, 0, 1,},
        };

        public static float[,] RotY(float y) => new[,]
        {
            {(float) Math.Cos(y), 0, (float) -Math.Sin(y), 0},
            {0, 1, 0, 0},
            {(float) Math.Sin(y), 0, (float) Math.Cos(y), 0},
            {0, 0, 0, 1,},
        };

        public static float[,] RotZ(float z) => new[,]
        {
            {(float) Math.Cos(z), (float) Math.Sin(z), 0, 0},
            {(float) -Math.Sin(z), (float) Math.Cos(z), 0, 0},
            {0, 0, 1, 0},
            {0, 0, 0, 1,},
        };

        public static float[,] TranslateEllipseXY(float a, float b, float t) => new[,]
        {
            {1f, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 1, 0},
            {(float) (a * Math.Cos(t)), (float) (b * Math.Sin(t)), 0, 1}
        };

        public static float[,] TranslateEllipseXZ(float a, float b, float t) => new[,]
        {
            {1f, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 1, 0},
            {(float) (a * Math.Cos(t)), 0, (float) (b * Math.Sin(t)), 1}
        };

        public static float[,] TranslateX(float i) => new[,]
        {
            {1, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 1, 0},
            {i, 0, 0, 1}
        };

        public static float[,] TranslateY(float i) => new[,]
        {
            {1, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 1, 0},
            {0, i, 0, 1}
        };

        public static float[,] TranslateZ(float i) => new[,]
        {
            {1, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 1, 0},
            {0, 0, i, 1}
        };

        public static float[] FigureCenter(float[,] mainFigure)
        {
            var verts = mainFigure.GetLength(1);
            return Enumerable.Range(0, verts)
                .Select(i => mainFigure.GetCol(i))
                .Aggregate(new float[4], (x, acc) => Add(acc, x))
                .Select(x => x / verts).ToArray();
        }
    }
}
