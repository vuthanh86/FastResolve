using System;
using System.Collections.Generic;

namespace FastDiContainer.Interfaces
{
    public interface IContainerBuilder
    {
        IRegistrationBuilder For(Type type);
        IRegistrationBuilder For<T>();
        IFastDiContainer Build(IList<RegistrationItem> registrationServices);
    }
}
