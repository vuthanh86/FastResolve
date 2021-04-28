using System;
using FastDiContainer.Interfaces;
using NUnit.Framework;

namespace FastDiContainer.Tests
{
    internal interface IFakeService
    {
        string DoSomething();
    }

    internal class FakeAService : IFakeService
    {
        #region Implementation of IFakeService

        public string DoSomething()
        {
            Console.WriteLine("FakeA");
            return GetType().FullName;
        }

        #endregion
    }

    internal class FakeBService : IFakeService
    {
        #region Implementation of IFakeService

        public string DoSomething()
        {
            Console.WriteLine("FakeB");
            return GetType().FullName;
        }

        #endregion
    }

    [TestFixture]
    public class FastDiContainerTests
    {
        [Test]
        public void FastDiContainerTDD()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.For(null);
            var container = containerBuilder.Build(null);

            // var service = container.Resolve<IRegistrationType>();

            // Assert.IsAssignableFrom<IRegistrationType>(service);
        }

        [Test]
        public void RegisterTest()
        {
            var container = new FastDiContainer();
            container.Register<IFakeService, FakeAService>();

            Assert.True(container.IsRegistered<IFakeService>(), $"{nameof(FakeAService)} was not registered.");
        }

        [Test]
        public void ResolveTest()
        {
            var container = new FastDiContainer();
            container.Register<IFakeService, FakeAService>();

            Assert.True(container.IsRegistered<IFakeService>(), $"{nameof(FakeAService)} was not registered.");

            var service = container.Resolve<IFakeService>();
            Assert.IsAssignableFrom<FakeAService>(service,
                $"Type of {nameof(service)} is not assignable from {typeof(FakeAService)}");

            var doSomething = service.DoSomething();
            Assert.True(doSomething.Equals(typeof(FakeAService).FullName));
        }
    }
}
