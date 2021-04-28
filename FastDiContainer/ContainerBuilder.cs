using System;
using System.Collections.Generic;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class ContainerBuilder : IContainerBuilder
    {
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
