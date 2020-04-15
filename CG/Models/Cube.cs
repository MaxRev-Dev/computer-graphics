using GraphicExtensions;
using Playground.Helpers;
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
        }

        public override void Draw(IProjectorEngine projector)
        {
            for (int i = 1; i < Model3D.GetLength(0); i++)
                // TODO: find out how to loop around all edges
                for (int j = i + 1; j < Model3D.GetLength(0); j++)
                {
                    var (x1, y1) = projector.ProjectVertexToScreen(Model3D.point(i));
                    var (x2, y2) = projector.ProjectVertexToScreen(Model3D.point(j));
                    projector.Graphics.DrawLine(PrimaryPen, x1, y1, x2, y2);
                }
        }
    }
}