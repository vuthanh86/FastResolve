using System;

namespace FastDiContainer.Interfaces
{
    public interface IScope : IDisposable, IServiceProvider
    {

    }
    public interface IServiceLifeTime : IScope
    {
        object GetServiceAsSingleton(Type itemType);
        object GetServicePerScope(Type itemType);
    }
}
