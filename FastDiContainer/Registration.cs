using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public abstract class RegistrationBase<T> : IRegistration
    {
        private readonly Type _registeredType;
        private readonly object _key;
        public RegistrationBase(object key = null)
        {
            _registeredType = typeof(T);
            _key = key;
        }
        public Type ReturnType => throw new NotImplementedException();

        public object Key => throw new NotImplementedException();

        public void PerScope()
        {
            throw new NotImplementedException();
        }

        public void SingleInstance()
        {
            throw new NotImplementedException();
        }
    }


    public class Registration : IRegistration
    {
        private readonly Type _itemType;
        private readonly Action<Func<IServiceLifeTime, object>> _registeredFactory;
        private readonly Func<IServiceLifeTime, object> _factory;

        public Type ReturnType => _itemType;

        public object Key => throw new NotImplementedException();

        public Registration(Type itemType, Action<Func<IServiceLifeTime, object>> registeredFactory, Func<IServiceLifeTime, object> factory)
        {
            _itemType = itemType;
            _registeredFactory = registeredFactory;
            _factory = factory;

            registeredFactory(_factory);
        }

        public void SingleInstance()
        {
            _registeredFactory(lifeTime => lifeTime.GetServiceAsSingleton(ReturnType, _factory));
        }

        public void PerScope()
        {
            _registeredFactory(lifeTime => lifeTime.GetServicePerScope(ReturnType, _factory));
        }
    }
}