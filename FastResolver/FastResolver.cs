using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FastContainer
{
    public class FastContainer : IFastContainer
    {
        private readonly IDictionary<Type, Func<Lazy<object>>> _typeMappers;

        public FastContainer()
        {
            _typeMappers = new ConcurrentDictionary<Type, Func<Lazy<object>>>();
        }

        #region Implementation of IResolver

        public IContainerBuilder For<TSource>()
        {
            throw new NotImplementedException();
        }

        public IContainerBuilder For(Type from)
        {
            throw new NotImplementedException();
        }

        public TSource Resolve<TSource>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type typeFrom)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ContainerBuilder : IContainerBuilder
    {
        private readonly Type sourceType;

        private static Func<Type, Lazy<object>> CreateInstance = (destinationType) =>
        {
            return new Lazy<object>();
        };
        private ContainerBuilder(Type sourceType)
        {
            this.sourceType = sourceType;
        }

        #region Implementation of IBuilder

        public IContainerBuilder Bind<TDestination>()
        {
            throw new NotImplementedException();
        }

        public IContainerBuilder Bind(Type destination)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public interface IFastContainer
    {
        IContainerBuilder For<TSource>();
        IContainerBuilder For(Type from);
        TSource Resolve<TSource>();
        object Resolve(Type typeFrom);
    }

    public interface IContainerBuilder
    {
        IContainerBuilder Bind<TDestination>();
        IContainerBuilder Bind(Type destination);
    }
}