using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FastDiContainer.Interfaces
{
    public interface IObjectCached
    {
        object GetCached(Type itemType, Func<IServiceLifeTime, object> factory, IServiceLifeTime lifeTime);
    }

    public abstract class ObjectCached : IObjectCached
    {
        private readonly ConcurrentDictionary<Type, object> _instanceCached;

        protected ObjectCached()
        {
            _instanceCached = new ConcurrentDictionary<Type, object>();
        }

        #region Implementation of IObjectCached

        public object GetCached(Type itemType, Func<IServiceLifeTime, object> factory, IServiceLifeTime lifeTime)
        {
            return _instanceCached.GetOrAdd(itemType, _ => factory(lifeTime));
        }

        #endregion

        public void Dispose()
        {
            foreach (var obj in _instanceCached.Values)
                (obj as IDisposable)?.Dispose();
        }
    }
}
