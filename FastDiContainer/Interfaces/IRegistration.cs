using System;

namespace FastDiContainer.Interfaces
{
    public interface IRegistration
    {
        Type ReturnType { get; }

        object Key { get; }

        void SingleInstance();

        void PerScope();
    }

}
