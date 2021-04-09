using FunctionZero.zBind.z;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace zBindTests
{
    [TestClass]
    public class UnitTest1 : INotifyPropertyChanged
    {
        private int testIntResult;

        public event PropertyChangedEventHandler PropertyChanged;

        [TestMethod]
        public void TestMethod1()
        {
            var binding = new Bind();
            binding.Source = this;
            binding.Expression = "TestIntResult = TestIntResult+4";
            binding.ProvideValue(null);
            TestIntResult = 5;
            Assert.AreEqual(9, TestIntResult);
        }

        public int TestIntResult
        {
            get
            {
                return testIntResult;
            }
            set
            {

                testIntResult = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TestIntResult)));
            }
        }
    }
}