`z:Bind` is a `xaml` markup extension for `Xamarin.Forms` that allows you to bind directly to an `expression` 

If you want to do things like this: (note the expression is enclosed inside quotes)
```xaml
<StackLayout 
	IsVisible="{z:Bind '(Item.Count != 0) AND (Status == \'Administrator\')'}" > ...
```

1. Install `FunctionZero.zBind` to your shared project
2. add  `xmlns:z="clr-namespace:FunctionZero.zBind.z;assembly=FunctionZero.zBind"`  
To your `xaml` page (or let Visual Studio do it for you)
3. That's all you need to do :)

Head over [here](https://github.com/Keflon/FunctionZero.zBindTestApp) for 
source code, documentation and a sample application
