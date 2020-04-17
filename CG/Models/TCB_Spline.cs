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

        public bool VisualizeAsPoints { get; set; }

        public int IntermediatePoints { get; set; } = 100;

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

                // previous
                var point1 = tcbSpline.KeyPoints[j];
                j = i + 1;
                if (j > tcbSpline.KeyPoints.Count - 1)
                {
                    j = tcbSpline.IsClosedSpline ? 0 : i;
                }
                // middle
                var point2 = tcbSpline.KeyPoints[j];

                j++;
                if (j > tcbSpline.KeyPoints.Count - 1)
                {
                    j = tcbSpline.IsClosedSpline ? 0 : i;
                }
                //next
                var point3 = tcbSpline.KeyPoints[j];

                var tension = tcbSpline.KeyPoints[i].Tension;
                var continuity = tcbSpline.KeyPoints[i].Continuity;
                var bias = tcbSpline.KeyPoints[i].Bias;

                // (0) - 1 (kps) - (2)
                var incoming = 0.5f * (1 - tension) *
                         ((1 + bias) * (1 - continuity) * (tcbSpline.KeyPoints[i].Position - point1.Position) +
                          (1 - bias) * (1 + continuity) * (point2.Position - tcbSpline.KeyPoints[i].Position));

                tension = point2.Tension;
                continuity = point2.Continuity;
                bias = point2.Bias;

                // (1) - 2 (kps) - (3) next
                var outgoing = 0.5f * (1 - tension) *
                         ((1 + bias) * (1 + continuity) * (point2.Position - tcbSpline.KeyPoints[i].Position) +
                          (1 - bias) * (1 - continuity) * (point3.Position - point2.Position));

                for (var k = 0; k < tcbSpline.IntermediatePoints; k++)
                {
                    var t = (float)k / (tcbSpline.IntermediatePoints - 1);
                    var v = Interpolate(t, tcbSpline.KeyPoints[i].Position, point2.Position, incoming, outgoing);
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
            /*var s = new Vector3(1*//*00f*//*, 1, 1);

            KeyPoints.Add(new SplinePoint(new Vector3(1, 28, 0) * s));
            KeyPoints.Add(new SplinePoint(new Vector3(1.5f, 96, 0) * s));
            KeyPoints.Add(new SplinePoint(new Vector3(2, 117, 0) * s));
            KeyPoints.Add(new SplinePoint(new Vector3(2.5f, 181, 0) * s));
            KeyPoints.Add(new SplinePoint(new Vector3(3, 106, 0) * s));
            return;*/
            int randomPoints = 10;

            // randomize on each redraw
            KeyPoints.Clear();
            var b = projector.Graphics.VisibleClipBounds;

            int prewX = 100, prewY = 200;
            int stepX = (int)((b.Width - prewX) / (randomPoints + 1));
            int stepY = (int)((b.Height - prewY) / (randomPoints + 2));

            for (int i = 0, rit = 1; i < randomPoints; i++, rit++)
            {
                var (c1, c2) = (_rand.Next(stepX * rit, stepX * (rit + 1)),
                    _rand.Next(stepY * rit, stepY * (rit + 3)));

                KeyPoints.Add(new SplinePoint
                {
                    Position = new Vector3(c1, c2, 0),
                    Bias = (float)_rand.NextDouble() * 2 + 1,
                    Continuity = (float)_rand.NextDouble() * 2 + 1,
                    Tension = (float)_rand.NextDouble() * 2 + 1,
                });
            }
        }

        public override void Draw(IProjectorEngine projector)
        {
            var s = 100;
            // draw keypoints
            foreach (var point in KeyPoints)
            {
                projector.Graphics.DrawEllipse(SecondaryPen, (point.Position.X) * s, (point.Position.Y), 3, 3);
            }

            // draw spline points
            using (var rx = GetSplinePoints().GetEnumerator())
            {
                if (!VisualizeAsPoints && !rx.MoveNext())
                    return;
                var current = rx.Current;

                while (rx.MoveNext())
                {
                    var point = rx.Current;
                    if (VisualizeAsPoints)
                    {
                        projector.Graphics.DrawEllipse(PrimaryPen, point.X, point.Y, 1, 1);
                    }
                    else
                    {
                        projector.Graphics.DrawLine(PrimaryPen, point.X * s, point.Y, current.X * s, current.Y);
                        current = point;
                    }
                }
            }
        }
    }

    public class SplinePoint
    {
        public SplinePoint()
        {
        }

        public SplinePoint(Vector3 position)
        {
            Position = position;
        }

        public SplinePoint(Vector3 position, float tension, float continuity, float bias)
        {
            Position = position;
            Tension = tension;
            Continuity = continuity;
            Bias = bias;
        }
        public Vector3 Position { get; set; }
        public float Tension { get; set; } = 1.7559526299369238988603263643669f;
        public float Continuity { get; set; } = 1.0158736798317970636275369716452f;
        public float Bias { get; set; } = 0.49603158004205073409311575708871f;
    }
}