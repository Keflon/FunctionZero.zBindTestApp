using FunctionZero.MvvmZero;
using FunctionZero.zBindTestApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.zBindTestApp.Mvvm.PageViewModels
{
    public class HomePageVm : MvvmZeroBaseVm
    {
        private long _count;
        private bool _isTest;
        private bool _isTest2;
        private double _sliderValue;

        public ObservableCollection<TestClass> ListDataSource { get; }

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
        public bool IsTest2
        {
            get => _isTest2;
            set => SetProperty(ref _isTest2, value);
        }
        public double SliderValue
        {
            get => _sliderValue;
            set => SetProperty(ref _sliderValue, value);
        }

        public HomePageVm()
        {
            TestInstance = new TestClass();
            SliderValue = -0.75;

            ListDataSource = new ObservableCollection<TestClass>();

            for(int c=0;c<5;c++)
            {
                ListDataSource.Add(new TestClass() { TestCountingLong = c});
            }
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
