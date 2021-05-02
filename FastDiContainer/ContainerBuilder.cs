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
        private Type _concreteType;


        public IContainerBuilder For(Type concreteType)
        {
            _concreteType = concreteType ?? throw new ArgumentNullException($"{nameof(concreteType)} can not be null.");
            return this;
        }

        public IContainerBuilder For<T>()
        {
            var concreteType = typeof(T);
            _concreteType = concreteType ?? throw new ArgumentNullException($"{nameof(concreteType)} can not be null.");
            return this;
        }

        public IRegistration Bind(Type itemType)
        {
            return Register(_concreteType, CreateInstanceFactory(itemType));
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

    }
}
