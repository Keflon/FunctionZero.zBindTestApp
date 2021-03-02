using FunctionZero.MvvmZero;

namespace FunctionZero.zBindTestApp.Mvvm.ViewModels
{
    public class TestClass : MvvmZeroBaseVm
    {
        private long _testLong;
        public long TestLong
        {
            get => _testLong;
            set => SetProperty(ref _testLong, value);
        }
    }
}
