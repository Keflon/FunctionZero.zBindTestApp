using FunctionZero.MvvmZero;
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

        public long Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public HomePageVm()
        {
        }

        public async Task InitAsync()
        {
            //throw new NotImplementedException();
        }

        private bool MyTimerCallback()
        {
            Count++;
            return base.IsOwnerPageVisible;
        }

        public override void OwnerPageAppearing()
        {
            base.OwnerPageAppearing();
            Device.StartTimer(TimeSpan.FromMilliseconds(22), MyTimerCallback);
        }
    }
}
