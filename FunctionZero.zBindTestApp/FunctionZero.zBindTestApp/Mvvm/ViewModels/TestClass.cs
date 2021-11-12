using FunctionZero.MvvmZero;
using System;
using Xamarin.Forms;

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

        private long _testCountingLong;
        public long TestCountingLong
        {
            get => _testCountingLong;
            set => SetProperty(ref _testCountingLong, value);
        }

        public TestClass()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(1000), TimerCallback);
        }

        private bool TimerCallback()
        {
            TestCountingLong++;
            return true;
        }
    }
}
