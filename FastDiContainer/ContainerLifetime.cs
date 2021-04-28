using System;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class ContainerLifetime : ObjectCached, IServiceLifeTime
    {

        public object GetService(Type itemType)
        {
            throw new NotImplementedException();
        }

        public object GetServiceAsSingleton(Type itemType)
        {
            throw new NotImplementedException();
        }

        public object GetServicePerScope(Type itemType)
        {
            throw new NotImplementedException();
        }
    }

    public class ScopeLifetime : ObjectCached, IServiceLifeTime
    {
        public object GetService(Type itemType)
        {
            throw new NotImplementedException();
        }

        public object GetServiceAsSingleton(Type itemType)
        {
            throw new NotImplementedException();
        }

        public object GetServicePerScope(Type itemType)
        {
            throw new NotImplementedException();
        }
    }
}