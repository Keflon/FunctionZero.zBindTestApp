`z:Bind` is a `xaml` markup extension for `Xamarin.Forms` that allows you to bind directly to an `expression` 

If you want to do things like this:
```xaml
<StackLayout 
	IsVisible="{z:Bind (Item.Count != 0) AND (Status == \'Administrator\')}" > ...
```

Install 

`FunctionZero.zBind` 

to your shared project and add 

`xmlns:z="clr-namespace:FunctionZero.zBind.z;assembly=FunctionZero.zBind"`

To your `xaml` page (or let Visual Studio do it for you). Simple as that!

Head over [here](https://github.com/Keflon/FunctionZero.zBindTestApp) for `source code`, `documentation` and a `sample application`
