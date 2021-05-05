using System;

namespace FastDiContainer.Interfaces
{
    public interface IRegistration
    {
        Type RegisteredType { get; set;}

        object Key { get; set;}

        void SingleInstance();

        void PerScope();

        Func<IServiceLifeTime, object> BuildFactory();

        bool Verify();
    }

}
