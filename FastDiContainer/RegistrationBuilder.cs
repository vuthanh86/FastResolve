using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class RegistrationBuilder : IRegistrationBuilder
    {
        private readonly Type _concreteType;
        private Type _registeredType;
        private readonly HashSet<Type> _registerTypes;

        public RegistrationBuilder(Type concreteType)
        {
            _registerTypes = new HashSet<Type>();
            _concreteType = concreteType;
        }

        public IRegistrationBuilder SingleInstance()
        {
            return this;
        }

        public IRegistrationBuilder Bind(Type aliasType)
        {
            _registeredType = aliasType;
            _registerTypes.Add(aliasType);
            return this;
        }

        public IRegistrationBuilder Bind<T>()
        {
            return Bind(typeof(T));
        }

        public IRegistrationBuilder BindSelf()
        {
            return Bind(_concreteType);
        }

        public IRegistrationBuilder PerScope()
        {
            return this;
        }

        public IRegistrationBuilder WithNamed(string name)
        {
            return this;
        }

        private RegistrationType Build()
        {

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