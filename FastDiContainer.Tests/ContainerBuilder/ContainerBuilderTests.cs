using NUnit.Framework;

namespace FastDiContainer.Tests.ContainerBuilder
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
            var containerBuilder = new global::FastDiContainer.ContainerBuilder();
            Assert.DoesNotThrow(() => {
                var r = containerBuilder.RegisterFor<IFakeService>().As<FakeAService>();
                Assert.That(r.ReturnType.Equals(typeof(IFakeService)), "Register type was not correct");

                var container = containerBuilder.Build();
                Assert.IsNotNull(container);

            }, "Register throw error");
        }
    }
}