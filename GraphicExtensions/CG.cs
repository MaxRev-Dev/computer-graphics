using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MaxRev.Extensions.Matrix;

namespace GraphicExtensions
{
    public static class CG
    {
        public static bool ContainsPoint(this IEnumerable<(float x, float y)> polygon, (float x, float y) p)
        {
            return ContainsPoint(polygon.Select(x => new Point(x.x, x.y)).ToArray(), new Point(p.x, p.y));
        }

        public static bool ContainsPoint(this Point[] polygon, Point p)
        {
            double minX = polygon[0].X;
            double maxX = polygon[0].X;
            double minY = polygon[0].Y;
            double maxY = polygon[0].Y;
            for (int i = 1; i < polygon.Length; i++)
            {
                Point q = polygon[i];
                minX = Math.Min(q.X, minX);
                maxX = Math.Max(q.X, maxX);
                minY = Math.Min(q.Y, minY);
                maxY = Math.Max(q.Y, maxY);
            }

            if (p.X < minX || p.X > maxX || p.Y < minY || p.Y > maxY)
            {
                return false;
            }

            // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) &&
                    p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }
        public static double Heading(this Vector v)
        {
            return Math.Atan2(v.Y, v.X);
        }
        public static Vector Rotate(this Vector v, double a)
        {
            var h = v.Heading();
            return new Vector(v.Length * Math.Cos(h + a), v.Length * Math.Sin(h + a));
        }

        public static float F(this double d) => (float)d;

        public static float[,] ApplyTransform(float[,] m, float[,] tr)
        {
            return m.Multiply(tr);
        }

        public static float[,] ExpandW(this float[,] matrix)
        {
            var ret = new float[4, 4];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    ret[i, j] = matrix[i, j];
                }
            }

            ret[3, 3] = 1;
            return ret;
        }

        public static float[,] DropW(this float[,] matrix)
        {
            var ret = new float[3, 3];
            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < matrix.GetLength(1) - 1; j++)
                {
                    ret[i, j] = matrix[i, j];
                }
            }

            return ret;
        }

        public static float[] MultiplyNorm(this float[] a, float[,] matrix)
        {
            return Multiply(a, matrix).Normalize();
        }

        public static float[] Multiply(this float[] a, float[,] matrix)
        {
            var res = new float[a.Length];
            for (int u = 0; u < a.Length; u++)
            {
                var i = 0;
                res[u] = a.Aggregate(0f, (acc, x) => acc + x * matrix[u, i++]);
            }
            return res;
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

        public static void setProjectionMatrix(float[,] projMatrix, float angleOfView, float near, float far)
        {
            float scale = (float)(1f / Math.Tan(angleOfView * 0.5 * Math.PI / 180));
            projMatrix[0, 0] = scale;
            projMatrix[1, 1] = scale;
            projMatrix[2, 2] = -far / (far - near);
            projMatrix[3, 2] = -far * near / (far - near);
            projMatrix[2, 3] = -1;
            projMatrix[3, 3] = 0;
        }

        public static float[,] lookAt(float[] eye, float[] center, float[] up)
        {
            var f = center.Subtract(eye).Normalize();
            var u = up.Normalize();
            var s = f.Cross(u).Normalize();
            u = Cross(s, f);
            var res = new float[4, 4];
            res[0, 0] = s[0];
            res[1, 0] = s[1];
            res[2, 0] = s[2];
            res[0, 1] = u[0];
            res[1, 1] = u[1];
            res[2, 1] = u[2];
            res[0, 2] = -f[0];
            res[1, 2] = -f[1];
            res[2, 2] = -f[2];
            res[3, 0] = -s.Dot(eye);
            res[3, 1] = -u.Dot(eye);
            res[3, 2] = f.Dot(eye);
            return res;
        }

        public static float[] ToArray(this (float x, float y, float z) array)
        {
            return new[] { array.x, array.y, array.z };
        }

        public static (float x, float y, float z) ToPoint3D(this float[] array)
        {
            return (array[0], array[1], array[2]);
        }
        public static (float x, float y) ToPoint2D(this float[] array)
        {
            return (array[0], array[1]);
        }

        public static (float x, float y, float z) point(this float[,] model, int index)
        {
            return (model[index, 0], model[index, 1], model[index, 2]);
        }

        public static float[] Normalize(this float[] a)
        {
            if (a.Any(x => Math.Abs(x) > 1))
            {
                var b = new float[a.Length];
                var m = (float)Math.Sqrt(a.Aggregate(0f, (acc, x) => acc + x * x));
                for (int i = 0; i < a.Length; i++)
                    b[i] = a[i] / m;
                return b;
            }

            return a;
        }

        public static float Dot(this float[] v1, float[] v2)
        {
            return v1.Multiply(v2);
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

        public static float[,] ScaleP(float i) => new[,]
        {
            {i, 0, 0, 0},
            {0, i, 0, 0},
            {0, 0, i, 0},
            {0, 0, 0, 1,},
        };


        public static float[,] RotX(float x) => new[,]
        {
            {1, 0, 0, 0},
            {0, (float) Math.Cos(x), (float) -Math.Sin(x), 0},
            {0, (float) Math.Sin(x), (float) Math.Cos(x), 0},
            {0, 0, 0, 1,},
        };

        public static float[,] RotY(float y) => new[,]
        {
            {(float) Math.Cos(y), 0, (float) Math.Sin(y), 0},
            {0, 1, 0, 0},
            {(float) -Math.Sin(y), 0, (float) Math.Cos(y), 0},
            {0, 0, 0, 1,},
        };

        public static float[,] RotZ(float z) => new[,]
        {
            {(float) Math.Cos(z), (float) -Math.Sin(z), 0, 0},
            {(float) Math.Sin(z), (float) Math.Cos(z), 0, 0},
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

        public static float[] FigureCenter(this float[,] model)
        {
            var verts = model.GetLength(0);
            return Enumerable.Range(0, verts)
                .Select(i => model.GetRow(i))
                .Aggregate(new float[4], (x, acc) => Add(acc, x))
                .Select(x => x / verts).ToArray();
        }
        public static float[] Polygon2DCenter(this float[][] polygon)
        {
            var verts = polygon.Length;
            return Enumerable.Range(0, verts)
                .Select(i => polygon[i])
                .Aggregate(new float[2], (x, acc) => Add(acc, x))
                .Select(x => x / verts).ToArray();
        }
        public static (float x, float y, float z) Polygon3DCenter(this (float x, float y, float z)[] polygon)
        {
            var verts = polygon.Length;
            return Enumerable.Range(0, verts)
                .Select(i => polygon[i])
                .Aggregate(new float[3], (x, acc) => Add(acc.ToArray(), x))
                .Select(x => x / verts).ToArray().ToPoint3D();
        }
    }
}
