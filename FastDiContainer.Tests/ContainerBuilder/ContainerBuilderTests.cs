using NUnit.Framework;

namespace FastDiContainer.Tests
{
    [TestFixture]
    public class ContainerBuilderTests
    {

        public void Setup()
        {

        }

        [Test]
        public void RegisterTest()
        {
            var containerBuilder = new ContainerBuilder();
            Assert.DoesNotThrow(() => {
                var r = containerBuilder.RegisterFor<FakeAService>().As<IFakeService>();
                Assert.That(r.ReturnType.Equals(typeof(IFakeService)), "Register type was not correct");

                var container = containerBuilder.Build();
                Assert.IsNotNull(container);
                var t = container.Resolve<IFakeService>();
                Assert.AreEqual(typeof(IFakeService), t.GetType());

            }, "Register throw error");
        }
    }
}