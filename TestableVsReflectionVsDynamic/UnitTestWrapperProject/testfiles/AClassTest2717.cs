using ConsoleApp1;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTestWrapperProject
{
    [TestFixture]
    public class AClassTest2717
    {
        private Mock<IA> MockA;

        private Mock<IB> MockB;

        private Mock<IC> MockC;

        private dynamic target;

        [SetUp]
        public void Setup()
        {
            MockA = new Mock<IA>();
            MockA.Setup(a => a.A()).Returns(1);
            MockB = new Mock<IB>();
            MockB.Setup(b => b.B()).Returns(1);
            MockC = new Mock<IC>();
            MockC.Setup(c => c.C()).Returns(1);
            target = new ObjectWrapper(new AClass());
            target.A = MockA.Object;
            target.B= MockB.Object;
            target.C = MockC.Object;
        }

        [Test]
        public void ShouldReturnCorrectWhenCallABC()
        {
            Assert.AreEqual(target.CallABC(), 3);
        }

        [Test]
        public void ShouldReturnCorrectWhenCallAB()
        {
            Assert.AreEqual(target.CallAB(), 2);
        }

        [Test]
        public void ShouldReturnCorrectWhenCallAC()
        {
            Assert.AreEqual(target.CallAC(), 2);
        }
    }
}
