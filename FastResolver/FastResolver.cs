using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FastResolve
{
    public class FastResolver : IResolver
    {
        private IDictionary<Type, Type> _typeMappers = new ConcurrentDictionary<Type, Type>();

        private IBuilder _builder;

        #region Implementation of IResolver

        public IBuilder Create()
        {
            // _builder ??= Builder.Build();
            // return _builder;
            return null;
        }

        public TSource Resolve<TSource>()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class Builder : IBuilder
    {
        private readonly IDictionary<Type, Type> _mappers;
        private readonly IBuilder _builder;
        private static readonly Func<IDictionary<Type, Type>, IBuilder> LazyBuilder = (types) =>
        {
            var b = new Lazy<IBuilder>(() => new Builder(types));
            return b.IsValueCreated ? b.Value : default(IBuilder);
        };

        private Builder(IDictionary<Type, Type> mappers)
        {
            _mappers = mappers;
            _builder = LazyBuilder(mappers);
        }

        public static IBuilder Build(IDictionary<Type, Type> mappers)
        {
            return LazyBuilder(mappers);
        }

        #region Implementation of IBuilder

        public IBuilder For<TSource>()
        {
            throw new NotImplementedException();
        }

        public IBuilder For(Type from)
        {
            throw new NotImplementedException();
        }

        public IBuilder Use<TDestination>()
        {
            throw new NotImplementedException();
        }

        public IBuilder Use(Type destination)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public interface IResolver
    {
        IBuilder Create();
        TSource Resolve<TSource>();
    }

    public interface IBuilder
    {
        IBuilder For<TSource>();
        IBuilder For(Type from);
        IBuilder Use<TDestination>();
        IBuilder Use(Type destination);
    }
}