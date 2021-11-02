using FunctionZero.MvvmZero;
using FunctionZero.zBindTestApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.zBindTestApp.Mvvm.PageViewModels
{
    public class HomePageVm : MvvmZeroBaseVm
    {
        private long _count;
        private bool _isTest;

        public long Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public bool IsTest
        {
            get => _isTest;
            set => SetProperty(ref _isTest, value);
        }

        public HomePageVm()
        {
            TestInstance = new TestClass();
        }

        public TestClass TestInstance { get; }
        /// <summary>
        /// Always null, used for test.
        /// </summary>
        public TestClass OtherTestInstance { get; }

        public async Task InitAsync()
        {
            // Called when the page is pushed
        }

        private bool MyTimerCallback()
        {
            Count++;
            IsTest = (Count & 64L) != 0;
            return base.IsOwnerPageVisible;
        }

        public override void OnOwnerPageAppearing()
        {
            base.OnOwnerPageAppearing();
            Device.StartTimer(TimeSpan.FromMilliseconds(22), MyTimerCallback);
        }
    }
}
