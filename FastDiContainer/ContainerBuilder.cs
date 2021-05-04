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
        private readonly Dictionary<Type, Func<IServiceLifeTime, object>> _registeredFactory = new Dictionary<Type, Func<IServiceLifeTime, object>>();
        private Type _registerType;
        private IList<Type> _aliasType;

        public ContainerBuilder()
        {
            _aliasType = new List<Type>();
        }

        public IContainerBuilder RegisterFor(Type registerType)
        {
            _registerType = registerType ?? throw new ArgumentNullException($"{nameof(registerType)} can not be null.");
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
            VerifyRecord(aliasType);

            var r = Register(_registerType, CreateInstanceFactory(aliasType));

            return r;
        }

        public IRegistration As<T>()
        {
            var itemType = typeof(T);
            return As(itemType);
        }

        public IFastContainer Build()
        {
            var c = new FastContainer(_registeredFactory);
            return c;
        }

        private IRegistration Register(Type itemType, Func<IServiceLifeTime, object> factory)
        {
            var registerType = new Registration(itemType, f => _registeredFactory[itemType] = f, factory);
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

        private void VerifyRecord(Type aliasType)
        {
            if (_registerType is null
                || aliasType is null)
                throw new ArgumentNullException("alias type null");

            if (aliasType.Equals(_registerType))
                throw new ArgumentException("alias type must not equal registered type");
        }
    }
}
