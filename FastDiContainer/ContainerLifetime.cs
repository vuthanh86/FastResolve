using System;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class ContainerLifetime : ObjectCached, IServiceLifeTime
    {
        private readonly Func<Type, Func<IServiceLifeTime, object>> _getFactory;

        public ContainerLifetime(Func<Type, Func<IServiceLifeTime, object>> getFactory)
        {
            _getFactory = getFactory;
        }

        public object GetService(Type itemType)
        {
            return _getFactory(itemType)(this);
        }

        public object GetServiceAsSingleton(Type itemType, Func<IServiceLifeTime, object> factory)
        {
            return GetCached(itemType, factory, this);
        }

        public object GetServicePerScope(Type itemType, Func<IServiceLifeTime, object> factory)
        {
            return GetServiceAsSingleton(itemType, factory);
        }
    }

    public class ScopeLifetime : ObjectCached, IServiceLifeTime
    {
        private readonly ContainerLifetime _parentContainer;

        public ScopeLifetime(ContainerLifetime parentContainer)
        {
            _parentContainer = parentContainer;
        }
        public object GetService(Type itemType)
        {
            return _parentContainer.GetService(itemType);
        }

        public object GetServiceAsSingleton(Type itemType, Func<IServiceLifeTime, object> factory)
        {
            return _parentContainer.GetServiceAsSingleton(itemType, factory);
        }

        public object GetServicePerScope(Type itemType, Func<IServiceLifeTime, object> factory)
        {
            return GetCached(itemType, factory, this);
        }
    }
}