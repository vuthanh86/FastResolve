using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FastResolve
{
    public interface IFastDiContainer
    {
        void Register<TSource, TTarget>();
        void Register(Type sourceType, Type targetType);
    }
    public class FastDiContainer : IFastDiContainer
    {
        private readonly IDictionary<string, Func<string, Lazy<object>>> _mappingTypes;
        public FastDiContainer()
        {
            _mappingTypes = new ConcurrentDictionary<string, Func<string, Lazy<object>>>();
        }
        #region Implementation of IFastDiContainer

        public void Register<TSource, TTarget>()
        {
            throw new NotImplementedException();
        }

        public void Register(Type sourceType, Type targetType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}