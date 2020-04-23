using System;
using Playground.Projections.Abstractions;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using MaxRev.Extensions.Matrix;
using Playground.Helpers.Abstractions;
using Playground.Helpers.Reflection;

namespace Playground.Models
{
    internal class TCBSpline : GraphicExtension
    {
        [Modifiable(RequiresReset = false)]
        public bool IsClosedSpline { get; set; }

        [Modifiable(RequiresReset = false)]
        public bool VisualizeAsPoints { get; set; }

        [Modifiable(Scaling = 1, Min = 1, Max = 1000)]
        public int IntermediatePoints { get; set; } = 100;

        [Modifiable]
        public bool Preserve3D { get; set; } = true;

        public List<SplinePoint> KeyPoints { get; } = new List<SplinePoint>();

        public IEnumerable<Vector3> GetSplinePoints()
        {
            var tcbSpline = this;

            var lineCount = tcbSpline.KeyPoints.Count - (tcbSpline.IsClosedSpline ? 0 : 1);

            for (var i = 0; i < lineCount; i++)
            {
                var (prev, current, next) = GetPointsAt(i);

                // https://www.engr.colostate.edu/ECE481A2/Readings/TCB.pdf
                // https://www.geometrictools.com/Documentation/KBSplines.pdf
                var tension = current.Tension;
                var continuity = current.Continuity;
                var bias = current.Bias;
                var outgoing = 0.5f * (1 - tension) *
                         ((1 + bias) * (1 + continuity) * (current.Position - prev.Position) +
                          (1 - bias) * (1 - continuity) * (next.Position - current.Position));


                // should we use outgoing vector i or i+1?
                (prev, current, next) = GetPointsAt(i);
                tension = current.Tension;
                continuity = current.Continuity;
                bias = current.Bias;
                var incoming = 0.5f * (1 - tension) *
                         ((1 + bias) * (1 - continuity) * (current.Position - prev.Position) +
                          (1 - bias) * (1 + continuity) * (next.Position - current.Position));

                for (var k = 0; k < tcbSpline.IntermediatePoints; k++)
                {
                    var t = (float)k / (tcbSpline.IntermediatePoints - 1);
                    yield return Interpolate(t, current.Position, next.Position, outgoing, incoming);
                }
            }
        }

        private (SplinePoint, SplinePoint, SplinePoint) GetPointsAt(int i)
        {
            var tcbSpline = this;
            var point = KeyPoints[i];

            // previous (last in collection or i-1)
            var prev = tcbSpline.KeyPoints[
                i - 1 >= 0 ?
                    i - 1 :
                    tcbSpline.IsClosedSpline ? tcbSpline.KeyPoints.Count - 1 : i];

            //next (first in collection or i+1) 
            var next = tcbSpline.KeyPoints[
                i + 1 <= tcbSpline.KeyPoints.Count - 1 ?
                    i + 1 :
                    tcbSpline.IsClosedSpline ? 0 : i];
            return (prev, point, next);
        }

        private Vector3 Interpolate(float t, Vector3 h0, Vector3 h1, Vector3 h2, Vector3 h3)
        {
            var t2 = t * t;
            var t3 = t2 * t;
            return h0 * (2.0f * t3 - 3.0f * t2 + 1.0f) +
                   h1 * (-2.0f * t3 + 3.0f * t2) +
                   h2 * (t3 - 2.0f * t2 + t) +
                   h3 * (t3 - t2);
        }

        private readonly Random _rand = new Random();

        public override void Reset(IProjectorEngine projector)
        {
            /*var s = new Vector3(100, 1, 1);

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

            var scaling = .015f;
            var offset = new Vector3(-10, -6, 0);
            int prewX = 100, prewY = 200;
            int stepX = (int)((b.Width - prewX) / (randomPoints + 2));
            int stepY = (int)((b.Height - prewY) / (randomPoints + 2));
            int stepZ = (int)(((b.Height + b.Width) / 2) / (randomPoints + 2));

            for (int i = 0, rit = 1; i < randomPoints; i++, rit++)
            {
                var (x, y, z) =
                    (_rand.Next(stepX * rit, stepX * (rit + 2)),
                    _rand.Next(stepY * rit, stepY * (rit + 2)),
                    Preserve3D ? _rand.Next(stepZ * rit, stepZ * (rit + 2)) : 0);

                KeyPoints.Add(new SplinePoint
                {
                    Position = new Vector3(x, y, z) * scaling + offset,
                    Bias = (float)_rand.NextDouble() * 2 - 1,
                    Continuity = (float)_rand.NextDouble() * 2 - 1,
                    Tension = (float)_rand.NextDouble() * 2 - 1,
                });
            }
            base.Reset(projector);
        }

        public override float[,] Model3D
        {
            get => KeyPoints.Select(x => new[] { x.Position.X, x.Position.Y, Preserve3D ? x.Position.Z : 0, 1 })
                .ToArray().Convert();
            protected set {
                for (int i = 0; i < value.GetLength(0); i++)
                {
                    var point = value.GetRow(i);
                    KeyPoints[i].Position = new Vector3(point[0], point[1], Preserve3D ? point[2] : 0);
                }
            }
        }
        public override void Draw(IProjectorEngine projector)
        {
            // draw keypoints
            foreach (var point in KeyPoints)
            {
                var (x, y) = projector.ProjectVertexToScreen(point.Position);
                projector.Graphics.DrawEllipse(SecondaryPen, x - 1, y - 1, 3, 3);
                DrawPointInfo(projector, point);
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
                    var (x1, y1) = projector.ProjectVertexToScreen(point);
                    if (VisualizeAsPoints)
                    {
                        projector.Graphics.DrawEllipse(PrimaryPen, x1, y1, 1, 1);
                    }
                    else
                    { 
                        projector.DrawLine(PrimaryPen, point, current);
                        current = point;
                    }
                }
            }
        }

        private void DrawPointInfo(IProjectorEngine projector, SplinePoint point)
        {
            var (x, y) = projector.ProjectVertexToScreen(point.Position);
            projector.Graphics.DrawString($"{point.Tension:F2},{point.Continuity:F2},{point.Bias:F2}",
                new Font(new FontFamily(GenericFontFamilies.Monospace), 8),
                Brushes.Black, x, y);
        }
    }

    public class SplinePoint
    {
        public Vector3 Position { get; set; }
        public float Tension { get; set; }
        public float Continuity { get; set; }
        public float Bias { get; set; }
    }
}