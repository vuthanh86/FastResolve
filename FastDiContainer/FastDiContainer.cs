using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FastResolve
{
    public interface IFastDiContainer
    {
        bool IsRegistered<TSource>();
        bool IsRegistered(Type typeSource);
        void Register<TSource, TTarget>();
        void Register(Type sourceType, Type targetType);
        TSource Resolve<TSource>();

    }
    public class FastDiContainer : IFastDiContainer
    {
        private readonly IDictionary<Type, Func<object>> _mappingTypes;
        
        public FastDiContainer()
        {
            _mappingTypes = new ConcurrentDictionary<Type, Func<object>>();
        }
        #region Implementation of IFastDiContainer

        public bool IsRegistered<TSource>()
        {
            return IsRegistered(typeof(TSource));
        }

        public bool IsRegistered(Type typeSource)
        {
            
            return _mappingTypes.ContainsKey(typeSource);
        }

        public void Register<TSource, TTarget>()
        {
            Register(typeof(TSource), typeof(TTarget));
        }

        public void Register(Type sourceType, Type targetType)
        {
            if (IsRegistered(sourceType))
                return;
            
            _mappingTypes.Add(sourceType, () => CreateInstance(sourceType));
        }

        public TSource Resolve<TSource>()
        {
            return (TSource) Resolve(typeof(TSource));
        }

        private object Resolve(Type sourceType)
        {
            if (!_mappingTypes.TryGetValue(sourceType, out var resolveFunc))
                throw new ArgumentException("Not found registered type");
            
            return resolveFunc.Invoke();
        }

        private object CreateInstance(Type sourceType)
        {
            //Getting first constructor of resolved type by reflection
            var firstConstructor = sourceType.GetConstructors().FirstOrDefault();

            if (firstConstructor == null)
                throw new ArgumentNullException($"Nulllllll");
            //Getting first constructor's parameter by reflection
            var constructorParameters = firstConstructor.GetParameters();

            //if no parameter found then we dont need to think about other resolved type from the parameter
            if (!constructorParameters.Any())
                return Activator.CreateInstance(sourceType); // returning an instance of resolved type

            //if our resolved type has constructor then again we have to resolve that types; 
            //so again we are calling our resolve method to resolve from constructor
            IList<object> parameterList = constructorParameters.Select(parameterToResolve => Resolve(parameterToResolve.ParameterType)).ToList();
            //invoking parameters to constructor
            return firstConstructor.Invoke(parameterList.ToArray());
        }

        #endregion
    }
}