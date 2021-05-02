using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public abstract class BaseRegistration
    {

    }
    public class Registration : IRegistration
    {
        private readonly Type _itemType;
        private readonly Action<Func<IServiceLifeTime, object>> _registeredFactory;
        private readonly Func<IServiceLifeTime, object> _factory;

        public Registration(Type itemType, Action<Func<IServiceLifeTime, object>> registeredFactory, Func<IServiceLifeTime, object> factory)
        {
            _itemType = itemType;
            _registeredFactory = registeredFactory;
            _factory = factory;

            registeredFactory(_factory);
        }

        public void SingleInstance()
        {
            _registeredFactory(lifeTime => lifeTime.GetServiceAsSingleton(_itemType, _factory));
        }

        public void PerScope()
        {
            _registeredFactory(lifeTime => lifeTime.GetServicePerScope(_itemType, _factory));
        }
    }
}