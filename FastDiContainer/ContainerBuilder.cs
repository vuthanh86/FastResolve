using System;
using System.Collections.Generic;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class ContainerBuilder : IContainerBuilder
    {
        private readonly Dictionary<Type, Func<IServiceLifeTime, object>> _registeredFactory = new Dictionary<Type, Func<IServiceLifeTime, object>>();
        private readonly ContainerLifetime _defaultLifetime;

        public ContainerBuilder() => _defaultLifetime = new ContainerLifetime();
     
        public IFastDiContainer Build(IList<RegistrationItem> registrationServices)
        {
            throw new NotImplementedException();
        }

        public IRegistrationBuilder For(Type type)
        {
            throw new NotImplementedException();
        }

        public IRegistrationBuilder For<T>()
        {
            throw new NotImplementedException();
        }
    }
}
