using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using GraphicExtensions;
using Playground.Helpers;
using Playground.Primitives;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class KochSnowflake : GraphicExtension
    {
        private List<KochLine> _lines = new List<KochLine>();

        public KochSnowflake(Pen foregroundPen)
        {
            PrimaryPen = foregroundPen ?? PrimaryPen;
        }
          
        public override void Draw(IProjectorEngine projector)
        {
            if (_lines.Count == 0) Reset(projector);
            foreach (var line in _lines)
            {
                DrawKochLine(projector.Graphics, line);
            }
        }

        private void DrawKochLine(Graphics graphics, KochLine kochLine)
        {
            if (!IsValid(graphics)) return;
            graphics.DrawLine(PrimaryPen,
                kochLine.Start.X.F(),
                kochLine.Start.Y.F(),
                kochLine.End.X.F(),
                kochLine.End.Y.F());
        }

        private bool IsValid(Graphics graphics)
        {
            return !graphics.VisibleClipBounds.IsEmpty;
        }

        public void NextGen()
        {
            var nextGeneration = new List<KochLine>(_lines.Count * 4);
            foreach (KochLine line in _lines) nextGeneration.AddRange(line.Gen());
            _lines = nextGeneration;
        }

        public override void Reset(IProjectorEngine projector)
        {
            _lines.Clear();

            var leftPoint = new Vector(100, 200);
            var rightPoint = new Vector(500, 200);
            var bottomPoint = new Vector(300, 500);

            var s1 = new KochLine(bottomPoint, leftPoint);
            var s2 = new KochLine(leftPoint, rightPoint);
            var s3 = new KochLine(rightPoint, bottomPoint);

            _lines.Add(s1);
            _lines.Add(s2);
            _lines.Add(s3);
            base.Reset(projector);
        }
    }
}