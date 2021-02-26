using FunctionZero.MvvmZero;
using FunctionZero.zBindTestApp.Mvvm.Pages;
using FunctionZero.zBindTestApp.Mvvm.PageViewModels;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.zBindTestApp.Boilerplate
{
    public class Locator
    {
        private static bool _created = false;
        public Locator(/* You can inject any platform-specific implementations here, for registration in the IoC container */)
        {
            if (_created == true)
                throw new InvalidOperationException("Attempt to create multiple locators. Did you forget to make MainActivity Single-Top?");

            var container = new Container();
            container.Options.ResolveUnregisteredConcreteTypes = false;
#if PRODUCTION
            container.Options.EnableAutoVerification = false;
#else
            container.Options.EnableAutoVerification = true;
#endif
            container.Register<App>(Lifestyle.Singleton);
            container.Register<IPageServiceZero>(() => new PageServiceZero(() => App.Current.MainPage.Navigation, (theType) => container.GetInstance(theType)), Lifestyle.Singleton);
            container.Register<HomePage>(Lifestyle.Singleton);
            container.Register<HomePageVm>(Lifestyle.Singleton);

            TheContainer = container;
        }

        public Container TheContainer { get; private set; }
    }
}
