using System;

namespace FastDiContainer.Interfaces
{
    public interface IRegistrationBuilder
    {
        IRegistrationBuilder Bind(Type aliasType);
        IRegistrationBuilder Bind<T>();
        IRegistrationBuilder BindSelf();
        IRegistrationBuilder WithNamed(string name);
        IRegistrationBuilder SingleInstance();

        IRegistrationBuilder PerScope();
    }

}
