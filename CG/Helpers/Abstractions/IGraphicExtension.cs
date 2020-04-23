using System.Drawing;
using Playground.Projections.Abstractions;

namespace Playground.Helpers.Abstractions
{
    internal interface IGraphicExtension
    {
        Pen PrimaryPen { get; set; }
        Pen SecondaryPen { get; set; }

        bool Enable { get; set; }
        bool Global { get; set; }
        float[,] Model3D { get; } 
        void Draw(IProjectorEngine projector);
        void Transform(float[,] trs);
        void Reset(IProjectorEngine projector);
    }
}