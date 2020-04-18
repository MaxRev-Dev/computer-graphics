using System.Drawing;
using GraphicExtensions;
using Playground.Projections.Abstractions;

namespace Playground.Helpers
{
    internal abstract class GraphicExtension : IGraphicExtension
    {
        public Pen PrimaryPen { get; set; } = Pens.Blue;
        public Pen SecondaryPen { get; set; } = Pens.DarkBlue;
        public virtual bool Enable { get; set; } = true;
        [Modifiable]
        public virtual bool Global { get; set; }
        public virtual float[,] Model3D { get; protected set; }
        public abstract void Draw(IProjectorEngine projector);
        public void Transform(float[,] trs)
        {
            Model3D = CG.ApplyTransform(Model3D, trs);
        }

        public virtual void Reset(IProjectorEngine projector)
        {
            if (Model3D == default) return;
            Transform(CG.RotY(.5f));
            Transform(CG.RotX(.5f));
            Transform(CG.TranslateZ(-5));
        }
    }
}