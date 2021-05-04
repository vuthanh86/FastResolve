using System;
using System.Collections.Generic;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class FastContainer : IFastContainer
    {
        private readonly ContainerLifetime _lifeTime;
        private readonly Dictionary<Type, Func<IServiceLifeTime, object>> _registeredTypes = new Dictionary<Type, Func<IServiceLifeTime, object>>();

        public FastContainer(Dictionary<Type, Func<IServiceLifeTime, object>> registeredTypes)
        {
            _registeredTypes = registeredTypes;
            _lifeTime = new ContainerLifetime(t => _registeredTypes[t]);
        }

        public int TotalRegisteredService => _registeredTypes.Count;

        public void Dispose()
        {
            _lifeTime.Dispose();
        }

        public object GetService(Type serviceType)
        {

            if (!_registeredTypes.TryGetValue(serviceType, out Func<IServiceLifeTime, object> registeredType))
            {
                return null;
            }

            return registeredType(_lifeTime);
        }

        public bool IsRegistered<T>()
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }
    }
}