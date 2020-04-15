using System;
using Playground.Helpers;
using Playground.Projections.Abstractions;
using System.Collections.Generic;
using System.Numerics;

namespace Playground.Models
{
    internal class TCB_Spline : GraphicExtension
    {
        public bool IsClosedSpline { get; set; }

        public int IntermediatePoints { get; set; } = 10;

        public List<SplinePoint> KeyPoints { get; } = new List<SplinePoint>();

        public IEnumerable<Vector3> GetSplinePoints()
        {
            var tcbSpline = this;

            var lineCount = tcbSpline.KeyPoints.Count - (tcbSpline.IsClosedSpline ? 0 : 1);

            for (var i = 0; i < lineCount; i++)
            {
                var j = i - 1;
                if (j < 0)
                {
                    j = tcbSpline.IsClosedSpline ? tcbSpline.KeyPoints.Count - 1 : i;
                }

                var point1 = tcbSpline.KeyPoints[j];
                j = i + 1;
                if (j > tcbSpline.KeyPoints.Count - 1)
                {
                    j = tcbSpline.IsClosedSpline ? 0 : i;
                }
                var point2 = tcbSpline.KeyPoints[j];

                j++;
                if (j > tcbSpline.KeyPoints.Count - 1)
                {
                    j = tcbSpline.IsClosedSpline ? 0 : i;
                }
                var point3 = tcbSpline.KeyPoints[j];

                var tension = tcbSpline.KeyPoints[i].Tension;
                var continuity = tcbSpline.KeyPoints[i].Continuity;
                var bias = tcbSpline.KeyPoints[i].Bias;

                var r1 = 0.5f * (1 - tension) *
                         ((1 + bias) * (1 - continuity) * (tcbSpline.KeyPoints[i].Position - point1.Position) +
                          (1 - bias) * (1 + continuity) * (point2.Position - tcbSpline.KeyPoints[i].Position));

                tension = point2.Tension;
                continuity = point2.Continuity;
                bias = point2.Bias;

                var r2 = 0.5f * (1 - tension) *
                         ((1 + bias) * (1 + continuity) * (point2.Position - tcbSpline.KeyPoints[i].Position) +
                          (1 - bias) * (1 - continuity) * (point3.Position - point2.Position));

                for (var k = 0; k < tcbSpline.IntermediatePoints; k++)
                {
                    var t = (float)k / (tcbSpline.IntermediatePoints - 1);
                    var v = Interpolate(t, tcbSpline.KeyPoints[i].Position, point2.Position, r1, r2);
                    yield return v;
                }
            }
        }

        private Vector3 Interpolate(float t, Vector3 p1, Vector3 p2, Vector3 r1, Vector3 r2)
        {
            return p1 * (2.0f * t * t * t - 3.0f * t * t + 1.0f) + r1 * (t * t * t - 2.0f * t * t + t) +
                   p2 * (-2.0f * t * t * t + 3.0f * t * t) + r2 * (t * t * t - t * t);
        }

        private readonly Random _rand = new Random();

        public override void Reset(IProjectorEngine projector)
        {
            int N = 10;
            IsClosedSpline = true;

            // randomize on each redraw
            KeyPoints.Clear();
            var b = projector.Graphics.VisibleClipBounds;

            int prewX = 100, prewY = 200;
            int stepX = (int)((b.Width - prewX) / (N + 1));
            int stepY = (int)((b.Height - prewY) / (N + 2));

            for (int i = 0, rit = 1; i < N; i++, rit++)
            {
                var (c1, c2) = (_rand.Next(stepX * rit, stepX * (rit + 1)),
                    _rand.Next(stepY * rit, stepY * (rit + 3)));
                KeyPoints.Add(new SplinePoint { Position = new Vector3(c1, c2, 0) });
            }
        }

        public override void Draw(IProjectorEngine projector)
        {
            using (var rx = GetSplinePoints().GetEnumerator())
            {
                if (rx.MoveNext())
                {
                    var current = rx.Current;

                    while (rx.MoveNext())
                    {
                        var point = rx.Current;
                        projector.Graphics.DrawLine(PrimaryPen, point.X, point.Y, current.X, current.Y);
                        current = point;
                    }
                }

            }

            foreach (var point in KeyPoints)
            {
                projector.Graphics.DrawEllipse(SecondaryPen, point.Position.X - 1, point.Position.Y - 1, 3, 3);
            }

        }
    }

    public class SplinePoint
    {
        public float Tension { get; set; } = 0.08f;
        public float Continuity { get; set; } = .06f;
        public float Bias { get; set; } = 0.07f;
        public Vector3 Position { get; set; }
    }
}