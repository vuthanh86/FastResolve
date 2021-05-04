using System;

namespace FastDiContainer.Interfaces
{
    public interface IScope : IDisposable, IServiceProvider
    {

    }

    public interface IFastContainer : IScope
    {
        int TotalRegisteredService { get; }
        bool IsRegistered<T>();
        T Resolve<T>();

    }
    public interface IServiceLifeTime : IScope
    {
        object GetServiceAsSingleton(Type itemType, Func<IServiceLifeTime, object> factory);
        object GetServicePerScope(Type itemType, Func<IServiceLifeTime, object> factory);
    }
}
