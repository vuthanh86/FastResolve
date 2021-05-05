using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{

    public class Registration : IRegistration, IEqualityComparer<Registration>
    {
        private readonly Action<Func<IServiceLifeTime, object>> _registeredFactory;
        private readonly Func<IServiceLifeTime, object> _factory;

        public Type RegisteredType { get ; set; }
        public object Key { get ; set; }

        public Registration(Type itemType, object key = null)
        {
            RegisteredType = itemType;
            Key = key;
        }

        // public Registration(Type itemType, object key,
        //                     Action<Func<IServiceLifeTime, object>> registeredFactory,
        //                     Func<IServiceLifeTime, object> factory) : this(itemType, key)
        // {
        //     _registeredFactory = registeredFactory;
        //     _factory = factory;

        //     registeredFactory(_factory);
        // }

        public void SingleInstance()
        {
            _registeredFactory(lifeTime => lifeTime.GetServiceAsSingleton(RegisteredType, _factory));
        }

        public void PerScope()
        {
            _registeredFactory(lifeTime => lifeTime.GetServicePerScope(RegisteredType, _factory));
        }

        public bool Equals(Registration x, Registration y)
        {
            if (x is null || y is null)
                return false;

            return ReferenceEquals(x, y) && x.Key.Equals(y.Key) && x.RegisteredType.Equals(x.RegisteredType);
        }

        public int GetHashCode(Registration obj)
        {
            unchecked
            {
                const int multiplier = 31;
                int hash = GetType().GetHashCode();

                hash = (hash * multiplier) + obj.GetHashCode();
                hash = (hash * multiplier) + (obj.Key == null ? 0 : obj.GetHashCode());

                return hash;
            }
        }

        public Func<IServiceLifeTime, object> BuildFactory()
        {
            if (!Verify())
                throw new Exception("Record type was not correct");

            if (Key != null)
            {
                var aliasType = Key as Type;
                RegisteredType = aliasType;
            }

            return CreateInstanceFactory(RegisteredType);
        }

        public bool Verify()
        {
            return true;
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
