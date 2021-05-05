using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class ContainerBuilder : IContainerBuilder
    {
        private Type _registerType;
        private Type _aliasType;

        private IRegistration _currentRegisterType;
        private readonly HashSet<Registration> _registeredType;

        public ContainerBuilder()
        {
            _registeredType = new HashSet<Registration>();
        }

        public IContainerBuilder RegisterFor(Type registerType)
        {
            _registerType = registerType ?? throw new ArgumentNullException($"{nameof(registerType)} can not be null.");

            _currentRegisterType = Register(registerType);

            _registeredType.Add(r);

            return this;
        }

        public IContainerBuilder RegisterFor<T>()
        {
            var type = typeof(T);
            _registerType = type ?? throw new ArgumentNullException($"{nameof(type)} can not be null.");
            return this;
        }

        public IRegistration As(Type aliasType)
        {
            _aliasType = aliasType;

            return Register(_registerType, _aliasType);
        }

        public IRegistration As<T>()
        {
            var itemType = typeof(T);
            return As(itemType);
        }

        public IRegistration AsSelf()
        {
            _aliasType = _registerType;

            return Register(_registerType);
        }

        public IFastContainer Build()
        {
            Dictionary<Type, Func<IServiceLifeTime, object>> finalRegisterItems = new Dictionary<Type, Func<IServiceLifeTime, object>>();
            if (_registeredType.Count > 0)
                finalRegisterItems = _registeredType.ToDictionary(k => k.Key as Type, v => v.BuildFactory());

            return new FastContainer(finalRegisterItems);
        }

        private IRegistration Register(Type itemType, object aliasType = null)
        {
            var registerType = new Registration(itemType, aliasType);

            _registeredType.Add(registerType);

            return registerType;
        }

        private static Func<IServiceLifeTime, object> CreateInstanceFactory(Type itemType)
        {
            // Get first constructor for the type
            var constructors = itemType.GetConstructors();
            if (constructors.Length == 0)
            {
                // If no public constructor found, search for an internal constructor
                constructors = itemType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            }
            var constructor = constructors.First();

            // Compile constructor call as a lambda expression
            var arg = Expression.Parameter(typeof(IServiceLifeTime));
            return (Func<IServiceLifeTime, object>)Expression.Lambda(
                Expression.New(constructor, constructor.GetParameters().Select(
                    param =>
                    {
                        var resolve = new Func<IServiceLifeTime, object>(
                            lifetime => lifetime.GetService(param.ParameterType));
                        return Expression.Convert(
                            Expression.Call(Expression.Constant(resolve.Target), resolve.Method, arg),
                            param.ParameterType);
                    })),
                arg).Compile();
        }
    }
}
