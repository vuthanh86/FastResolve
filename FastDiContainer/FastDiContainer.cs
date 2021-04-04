using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FastDiContainer
{
    public interface IFastDiContainer
    {
        bool IsRegistered<T>();
        void Register<TConcrete, TDerived>() where TDerived : class, TConcrete;
        T Resolve<T>();

    }
    public class FastDiContainer : IFastDiContainer
    {
        private readonly IDictionary<Type, Func<object>> _mappingTypes;
        private readonly IDictionary<Type, Type> _registerTypes;
        private readonly IDictionary<Type, Func<object>> _instanceBindings;

        public FastDiContainer()
        {
            _registerTypes = new Dictionary<Type, Type>();
            _instanceBindings = new Dictionary<Type, Func<object>>();
            _mappingTypes = new ConcurrentDictionary<Type, Func<object>>();
        }
        #region Implementation of IFastDiContainer

        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        private bool IsRegistered(Type type)
        {
            return _mappingTypes.ContainsKey(type);
        }

        public void Register<TConcrete, TDerived>() where TDerived : class, TConcrete
        {
            Register(concreteType: typeof(TConcrete), derivedType: typeof(TDerived));
        }

        private void Register(Type concreteType, Type derivedType)
        {
            if (!concreteType.IsInterface ||
                !derivedType.IsClass ||
                !concreteType.IsAssignableFrom(derivedType))
                throw new ArgumentException($"{nameof(concreteType)} type was not represented by {derivedType}.");

            if (IsRegistered(derivedType))
                throw new ArgumentException($"{nameof(derivedType)} already registered.");
            
            _mappingTypes.Add(concreteType, () => CreateInstance(derivedType));
        }

        public T Resolve<T>()
        {
            return (T) Resolve(typeof(T));
        }

        private object Resolve(Type sourceType)
        {
            if (!_mappingTypes.TryGetValue(sourceType, out var resolveFunc))
                throw new KeyNotFoundException($"Resolve type: {sourceType.Name} error. Type was not found");

            return resolveFunc();
        }

        private object CreateInstance(Type sourceType)
        {
            var paramters = sourceType.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Count())
                .First()
                .GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            return Activator.CreateInstance(sourceType, paramters);
        }

        #endregion
    }
}
