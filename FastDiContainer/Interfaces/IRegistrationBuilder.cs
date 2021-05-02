using System;

namespace FastDiContainer.Interfaces
{
    public interface IRegistration
    {
        void SingleInstance();

        void PerScope();
    }

}
