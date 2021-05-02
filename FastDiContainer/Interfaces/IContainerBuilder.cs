using System;
using System.Collections.Generic;

namespace FastDiContainer.Interfaces
{
    public interface IContainerBuilder
    {
        IContainerBuilder For(Type concreteType);
        IContainerBuilder For<T>();
        IFastContainer Build();
    }
}
