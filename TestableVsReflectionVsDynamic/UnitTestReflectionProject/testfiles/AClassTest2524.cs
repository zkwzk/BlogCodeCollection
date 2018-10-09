using ConsoleApp1;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTestReflectionProject
{
    [TestFixture]
    public class AClassTest2524
    {
        private Mock<IA> MockA;

        private Mock<IB> MockB;

        private Mock<IC> MockC;

        private AClass target;

        [SetUp]
        public void Setup()
        {
            MockA = new Mock<IA>();
            MockA.Setup(a => a.A()).Returns(1);
            MockB = new Mock<IB>();
            MockB.Setup(b => b.B()).Returns(1);
            MockC = new Mock<IC>();
            MockC.Setup(c => c.C()).Returns(1);
            target = new AClass();
            target.SetPrivateProperty<AClass, IA>("A", MockA.Object);
            target.SetPrivateProperty<AClass, IB>("B", MockB.Object);
            target.SetPrivateProperty<AClass, IC>("C", MockC.Object);
        }

        [Test]
        public void ShouldReturnCorrectWhenCallABC()
        {
            Assert.AreEqual(target.CallABC(), 3);
        }

        [Test]
        public void ShouldReturnCorrectWhenCallAB()
        {
            Assert.AreEqual(target.InvokeNonPublicMethod<int>("CallAB", (object[])null), 2);
        }

        [Test]
        public void ShouldReturnCorrectWhenCallAC()
        {
            Assert.AreEqual(target.InvokeNonPublicMethod<int>("CallAC", (object[])null), 2);
        }
    }
}
