using FunctionZero.zBind.f;
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
            var binding = new Bind(this, nameof(TestIntResult));
            TestIntResult = 5;
            Assert.AreEqual(5, binding.Value);
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