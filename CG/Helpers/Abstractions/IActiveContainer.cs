using System.Collections.Generic;

namespace Playground.Helpers.Abstractions
{
    internal interface IActiveContainer<T> : IEnumerable<T>
    {
        void ActiveChanged(T projector); 
        T1 Get<T1>() where T1 : class;
    }
}