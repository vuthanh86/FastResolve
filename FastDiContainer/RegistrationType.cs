using System;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class RegistrationType
    {
        private readonly Type _concreteType;
        private readonly Action<Func<LifeTime, object>> _registeredFactory;
        private readonly LifeTime _lifeTime;
        private readonly Func<LifeTime, object> _factory;

        public RegistrationType(Type concreteType, Action<Func<LifeTime, object>> registeredFactory, Func<LifeTime, object> factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _concreteType = concreteType ?? throw new ArgumentNullException(nameof(concreteType));
            this._registeredFactory = registeredFactory ?? throw new ArgumentNullException(nameof(registeredFactory));
            this._factory = factory;

            registeredFactory(factory);
        }

        public Type ConcreteType => _concreteType;

        public object GetInstance()
        {
            return _factory(_lifeTime);
        }
    }
}
