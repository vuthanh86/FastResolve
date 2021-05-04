using System;
using System.Collections.Generic;

namespace FastDiContainer.Interfaces
{
    public interface IContainerBuilder
    {
        IContainerBuilder RegisterFor(Type registerType);
        IContainerBuilder RegisterFor<T>();
        IRegistration As(Type aliasType);
        IRegistration As<T>();
        IFastContainer Build();
    }
}
