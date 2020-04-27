using System.Drawing;
using GraphicsExtensions;
using Playground.Helpers.Reflection;
using Playground.Projections.Abstractions;

namespace Playground.Helpers.Abstractions
{
    internal abstract class GraphicExtension : IGraphicExtension
    {
        public Pen PrimaryPen { get; set; } = Pens.Blue;
        public Pen SecondaryPen { get; set; } = Pens.DarkBlue;
        public virtual bool Enable { get; set; } = true;
        [Modifiable(RequiresReset = false)]
        public virtual bool Global { get; set; }
        public virtual float[,] Model3D { get; protected set; }
        public abstract void Draw(IProjectorEngine projector);
        public void Transform(float[,] trs)
        {
            Model3D = CG.ApplyTransform(CheckW(Model3D), trs);
        }

        private float[,] CheckW(float[,] model3D)
        {
            return model3D.GetLength(1) != 4 ? model3D.ExpandW() : model3D;
        }

        public virtual void Reset(IProjectorEngine projector)
        {
            if (Model3D == default) return;
            Transform(CG.RotY(.5f));
            Transform(CG.RotX(.5f));
            Transform(CG.TranslateZ(-12));
        }
    }
}