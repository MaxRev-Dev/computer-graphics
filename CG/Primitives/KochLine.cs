using System;
using System.Collections.Generic;
using System.Windows;
using GraphicExtensions;

namespace Playground.Primitives
{
    internal class KochLine
    {
        public KochLine(Vector start, Vector end)
        {
            Start = start;
            End = end;
        }

        public Vector Start { get; }
        public Vector End { get; }

        public IEnumerable<KochLine> Gen()
        {
            var v = Vector.Subtract(End, Start);
            v = Vector.Divide(v, 3);
            // left, top, right
            var lv = Vector.Add(Start, v);
            yield return new KochLine(Start, lv);
            var rv = Vector.Subtract(End, v);
            yield return new KochLine(rv, End);
            v = v.Rotate(-Math.PI / 3);
            var tv = Vector.Add(lv, v);
            yield return new KochLine(lv, tv);
            yield return new KochLine(tv, rv);
        }
    }
}