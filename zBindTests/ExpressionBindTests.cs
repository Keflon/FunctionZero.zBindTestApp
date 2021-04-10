using FunctionZero.yBind;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace zBindTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestExpression()
        {
            var host = new TestClass(null, 5);
            var binding = new ExpressionBind(host, "TestIntResult * 2");
            Assert.AreEqual(10, (int)(long)binding.Result);

            host.TestIntResult++;
            Assert.AreEqual(12, (int)(long)binding.Result);
        }

        [TestMethod]
        public void TestAutoExpression()
        {
            var host = new TestClass(null, 5);
            var binding = new ExpressionBind(host, "TestIntResult * 2");
            Assert.AreEqual(10, (int)(long)binding.Result);

            host.TestIntResult++;
            Assert.AreEqual(12, (int)(long)binding.Result);
        }

        [TestMethod]
        public void TestNestedExpression()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 41), 6), 5);

            var binding = new ExpressionBind(host, $"(Child.TestIntResult + Child.Child.TestIntResult) * TestIntResult");
            Assert.AreEqual((6 + 41) * 5, (int)(long)binding.Result);

            host.Child.TestIntResult++;
            Assert.AreEqual((7 + 41) * 5, (int)(long)binding.Result);
        }

        [TestMethod]
        public void TestAutoNestedExpression()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 41), 6), 5);

            var binding = new ExpressionBind(host, $"(Child.TestIntResult + Child.Child.TestIntResult) * TestIntResult");
            Assert.AreEqual((6 + 41) * 5, (int)(long)binding.Result);

            host.Child.TestIntResult++;
            Assert.AreEqual((7 + 41) * 5, (int)(long)binding.Result);
        }
    }
}