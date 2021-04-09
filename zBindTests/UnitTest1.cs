using FunctionZero.zBind.f;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace zBindTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBindToInt()
        {
            var host = new TestClass(null, 5);
            var binding = new Bind(host, nameof(TestClass.TestIntResult));
            Assert.AreEqual(5, binding.Value);

            host.TestIntResult++;
            Assert.AreEqual(6, binding.Value);
        }

        [TestMethod]
        public void TestBindToNestedInt()
        {
            var host = new TestClass(new TestClass(null, 6), 5);
            var binding = new Bind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.TestIntResult)}");
            Assert.AreEqual(6, binding.Value);

            host.Child.TestIntResult++;
            Assert.AreEqual(7, binding.Value);
        }

        [TestMethod]
        public void TestBindToReplacedNestedInt()
        {
            var host = new TestClass(new TestClass(null, 6), 5);
            var binding = new Bind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.TestIntResult)}");
            Assert.AreEqual(6, binding.Value);

            host.Child.TestIntResult++;
            Assert.AreEqual(7, binding.Value);

            host.Child = new TestClass(null, -11);
        }
    }
}