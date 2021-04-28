using System;
using FastDiContainer.Interfaces;

namespace FastDiContainer
{
    public class FastContainer : IScope
    {
        public FastContainer()
        {

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}