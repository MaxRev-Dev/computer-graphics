using GraphicsExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Helpers.Abstractions;
using Playground.Helpers.Reflection;
using Playground.Primitives;
using Playground.Projections.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Playground.Models
{
    internal class KochSnowflake : GraphicExtension
    {
        private List<KochLine> _lines = new List<KochLine>();
        private readonly Dictionary<KochLine, (float[] S, float[] E)> _3dState
            = new Dictionary<KochLine, (float[] S, float[] E)>();
        public override void Draw(IProjectorEngine projector)
        {
            if (_lines.Count == 0) Reset(projector);
            foreach (var line in _lines)
            {
                DrawKochLine(projector, line);
            }
        }

        public override float[,] Model3D
        {
            get => _lines.Select(Project3d).SelectMany(x => x).ToArray().Convert();
            protected set => SetLines(value);
        }

        private void SetLines(float[,] value)
        {
            ClearState();
            for (int i = 0; i < value.GetLength(0) - 1; i += 2)
            {
                var s = value.GetRow(i);
                var e = value.GetRow(i + 1);
                var key = new KochLine(s.ToVector2D(), e.ToVector2D());
                _lines.Add(key);
                _3dState[key] = (s, e);
            }
        }

        private IEnumerable<float[]> Project3d(KochLine arg)
        {
            if (_3dState.ContainsKey(arg))
            {
                yield return _3dState[arg].S;
                yield return _3dState[arg].E;
                yield break;
            }

            var s = arg.Start.ToArray3D();
            var e = arg.End.ToArray3D();
            _3dState[arg] = (s, e);
            yield return s;
            yield return e;
        }

        private void DrawKochLine(IProjectorEngine projector, KochLine kochLine)
        { 
            var state = _3dState[kochLine];
            projector.DrawLine(PrimaryPen, state.S.ToPoint3D(), state.E.ToPoint3D());
        }

        public void NextGen()
        {
            var nextGeneration = new List<KochLine>(_lines.Count * 4);
            foreach (KochLine line in _lines) nextGeneration.AddRange(line.Gen());
            _lines = nextGeneration;
        }

        public override void Reset(IProjectorEngine projector)
        {
            ClearState();

            var v1 = new Vector(.5, 0);
            var lv = v1.Rotate(Math.PI / 2);
            var rv = v1.Rotate(-Math.PI / 2);

            _lines.Add(new KochLine(v1, lv));
            _lines.Add(new KochLine(lv, rv));
            _lines.Add(new KochLine(rv, v1));
            base.Reset(projector);
        }

        private void ClearState()
        {
            _lines.Clear();
            _3dState.Clear();
        }
    }
}