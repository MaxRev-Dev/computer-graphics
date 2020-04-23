using System.Collections.Generic;
using System.Linq;
using Playground.Helpers.Abstractions;
using Playground.Projections.Abstractions;

namespace Playground.Helpers.Containers
{
    internal class ProjectorContainer : List<IProjectorEngine>, IActiveContainer<IProjectorEngine>
    {
        public IProjectorEngine Current { get; private set; }

        T IActiveContainer<IProjectorEngine>.Get<T>() where T : class
        {
            return this.FirstOrDefault(c => c is T) as T;
        }

        public void InitializeAll(GraphicContext context)
        {
            ForEach(x => x.Use(context));
        }

        public void ActiveChanged(IProjectorEngine projector)
        {
            if (projector != default && Current?.Context != default)
                Current = projector.Use(Current.Context);
        }

        public void Use<T>() where T : class
        {
            Current = ((IActiveContainer<IProjectorEngine>)this).Get<T>() as IProjectorEngine;
        }
    }
}