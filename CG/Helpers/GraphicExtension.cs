using System.Drawing;
using Playground.Projections.Abstractions;

namespace Playground.Helpers
{
    internal abstract class GraphicExtension : IGraphicExtension
    {
        public Pen PrimaryPen { get; set; } = Pens.Blue;
        public Pen SecondaryPen { get; set; } = Pens.DarkBlue;
        public virtual bool Enable { get; set; } = true;
        public virtual float[,] Model3D { get; protected set; } 
        public abstract void Draw(IProjectorEngine projector);
        public void Transform(float[,] trs)
        {
            Model3D = GraphicExtensions.CG.ApplyTransform(Model3D, trs);
        }

        public virtual void Reset(IProjectorEngine projector)
        { 
        }
    }
}