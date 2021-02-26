using FunctionZero.MvvmZero;
using FunctionZero.zBindTestApp.Mvvm.Pages;
using FunctionZero.zBindTestApp.Mvvm.PageViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FunctionZero.zBindTestApp
{
    public partial class App : Application
    {
        public App(IPageServiceZero pageService)
        {
            InitializeComponent();
            MainPage = pageService.MakePage<HomePage, HomePageVm>(async (vm) => await vm.InitAsync());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
