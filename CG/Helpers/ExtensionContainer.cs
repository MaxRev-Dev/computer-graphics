using System.Collections.Generic;
using System.Linq;
using Playground.Projections.Abstractions;

namespace Playground.Helpers
{
    internal class ExtensionContainer : List<IGraphicExtension>
    {
        public void DrawAll(IProjectorEngine projector)
        {
            foreach (var extension in this)
            {
                if (extension.Enable)
                    extension.Draw(projector);
            }
        }

        public T Get<T>() where T : class
        {
            return this.FirstOrDefault(c => c is T) as T;
        }

        public void InitializeAll(IProjectorEngine projector)
        {
            ForEach(x => x.Reset(projector));
        }

        public void ApplyTransformation(float[,] trs)
        {
            foreach (var v in this.Where(x => x.Model3D != default))
                v.Transform(trs);
        }
    }
}