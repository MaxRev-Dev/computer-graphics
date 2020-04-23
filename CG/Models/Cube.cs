using GraphicsExtensions;
using Playground.Helpers.Abstractions;
using Playground.Projections.Abstractions;

namespace Playground.Models
{
    internal class Cube : GraphicExtension
    {
        public override void Reset(IProjectorEngine projector)
        {
            Model3D = new[,]
            {
                {0f, 0, 1, 1},
                {1f, 0, 1, 1},
                {1f, 0.5f, 1, 1},
                {0.5f, 1, 1, 1},
                {0f, 1, 1, 1},
                {0f, 0, 0, 1},
                {1f, 0, 0, 1},
                {1f, 1, 0, 1},
                {0f, 1, 0, 1},
                {1f, 1, 0.5f, 1},
            };
            base.Reset(projector);
        }

        public override void Draw(IProjectorEngine projector)
        {
            for (int i = 1; i < Model3D.GetLength(0); i++)
                // TODO: find out how to loop around all edges
                for (int j = i + 1; j < Model3D.GetLength(0); j++)
                { 
                    projector.DrawLine(PrimaryPen, Model3D.point(i), Model3D.point(j));
                }
        }
    }
}