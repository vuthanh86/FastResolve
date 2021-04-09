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
        void Register<TDerived>() where TDerived : class;
        T Resolve<T>();

    }
    public class FastDiContainer : IFastDiContainer
    {
        private readonly IDictionary<Type, Func<object>> _mappingTypes;
        private readonly IDictionary<Type, Type> _registerTypes;

        public FastDiContainer()
        {
            _registerTypes = new Dictionary<Type, Type>();
            _mappingTypes = new ConcurrentDictionary<Type, Func<object>>();
        }
        #region Implementation of IFastDiContainer

        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        private bool IsRegistered(Type type)
        {
            return _registerTypes.ContainsKey(type) || _mappingTypes.ContainsKey(type);
        }

        public void Register<TConcrete, TDerived>() where TDerived : class, TConcrete
        {
            if (ReferenceEquals(null, typeof(TConcrete)) || ReferenceEquals(null, typeof(TDerived)))
                throw new ArgumentNullException($"{nameof(TConcrete)}/{nameof(TDerived)} can not be null");

            if (_registerTypes.ContainsKey(typeof(TConcrete)) && _mappingTypes.ContainsKey(typeof(TDerived)))
                throw new ArgumentException($"{typeof(TDerived)} already registered.");

            _registerTypes[typeof(TConcrete)] = typeof(TDerived);

            Register(key: typeof(TDerived), instanceFunc: () => CreateInstance(typeof(TDerived)));
        }

        public void Register<TDerived>() where TDerived : class
        {
            throw new NotImplementedException();
        }

        private void Register(Type key, Func<object> instanceFunc)
        {
            _mappingTypes.Add(key, instanceFunc);
        }

        public T Resolve<T>()
        {
            return (T) Resolve(typeof(T));
        }

        private object Resolve(Type sourceType)
        {
            if (_registerTypes.TryGetValue(sourceType, out var targetType))
            {
                if (_mappingTypes.TryGetValue(targetType, out var instanceFunc))
                {
                    return instanceFunc();
                }
            }
            else  if (_mappingTypes.TryGetValue(sourceType, out var instanceFunc))
            {

                return instanceFunc();
            }
            throw new Exception($"Resolve type: {sourceType.Name} error. Type was not found");
        }

        private object CreateInstance(Type targetType)
        {
            var parameters = targetType.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Count())
                .First()
                .GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            return Activator.CreateInstance(targetType, parameters);
        }

        #endregion
    }
}
