# FunctionZero.zBind

## Release notes for 1.2.1
All csharp value types supported including nullable types.  
Casting is supported. Cast to any type found in `ExpressionParserZero.Operands.OperandType`, 
e.g. `(NullableULong) 42`

## Overview
`z:Bind` is a `xaml` markup extension that allows you to bind directly to an `expression` 
rather than just a `property`

Put simply, it allows you to do things like this:
```xaml
<StackLayout 
	IsVisible="{z:Bind (Item.Count != 0) AND (Status == \'Administrator\'}" > ...
```

`Expressions` are re-evaluated only if a `property` on which they depend changes

## Features

- Full parsing and evaluation of ViewModel and/or view properties
- Alisaing of operators, so you can represent (for example) `&&` in xaml as `AND` rather than `&amp;&amp;`
- Registration of methods that can be called directly from `xaml`

## Quickstart

Use the package manager to add the NugetPackage `FunctionZero.zBind`

Add a namespace alias to your xaml, like this:
```xaml 
xmlns:z="clr-namespace:FunctionZero.zBind.z;assembly=FunctionZero.zBind"
```
And that's all there is to it, you can now `z:Bind` to any properties in your ViewModel

## Usage

Just like the `Xamarin.Forms` `Binding` object, `z:Bind` binds to properties in your `BindingContext` 
by default, 
or you can specify any suitable alternative by setting the `Source` property:

|Sample|Notes|
|--|:--:|
|`{z:Bind Count}`| Bind to `Count`|
|`{z:Bind IsExpanded, Source={x:Reference MyExpander}}`|Bind to a property on an element named  `MyExpander`|
|`{z:Bind Count * 2}`| Bind to an expression that yields Count * 2|
|`{z:Bind (Count * 2) LT 10}`| True if (Count * 2) < 10|
|`{z:Bind Sin(Count / 25.0)}`| Calls a _function_ (see below)|

## The Great Escape, and other cautionary tales
### Commas
If your _expression string_ has commas in it, you must hide them from the xaml parser, otherwise `z:Bind` will be given an incomplete string and things won't work as expected. 

You can do this by enclosing the string inside quotes, like this:
```xaml
{z:Bind 'SomeFunction(param1, param2)'}
```
Similar to `xaml`, string literals can be enclosed within 'single quotes' or "double quotes" and appropriate use 
of `xml` escape-sequences.
### Casting
Functions expect their parameters to be of the correct type, so `Sin(someFloat)` will throw an exception. 
`Sin((Double)someFloat)` is here to help.  
See `ExpressionParserZero.Operands.OperandType` for all the types you can cast to.

### Short-circuit
Just like `c#`, the underlying expression parser supports short-circuit, so expressions like 
`(thing != null) AND (thing.part == 5)` will work even if `thing` is `null`
### Errors
Error reporting is quite good, so check the debug output if things aren't working as expected.

### Aliases supported to simplify xaml

|Alias|Operator|
|--|:--:|
|NOT|!|
|MOD|%|
|LT|<|
|GT|>|
|GTE|>=|
|LTE|<=|
|BAND|&| 
|XOR|^| 
|BOR|\||
|AND|&&|
|OR| \|\||

## Supported value types
All `csharp` value types are supported, see the enum `ExpressionParserZero.Operands.OperandType` 
for a complete list

### Supported reference types
`string`, `object`

### Advanced Usage - Functions, aliases and operator-overloads
 `z:Bind` uses [`FunctionZero.ExpressionParserZero`](https://www.nuget.org/packages/FunctionZero.ExpressionParserZero) 
to do the heavy lifting, so take a look at the [documentation](https://github.com/Keflon/FunctionZero.ExpressionParserZero)
if you want to take a deeper dive. Here is a taster ...

### Functions
`Sin`, `Cos` and `Tan` and more are 
[registered by default](https://github.com/Keflon/FunctionZero.ExpressionParserZero#functions), 
as are the _aliases_ listed above.  
```xaml
<Label TranslationX="{z:Bind Sin(Count / 25.0) * 100.0}" ...
```
**Note: `Lerp` is also pre-registered; the following is just for example.**
Suppose you wanted a new function to to do a linear interpolation between two values, like this:
```csharp
double Lerp(double a, double b, double t)
{
  return a + t * (b - a);
}
```
For use like this: (Note the expression is enclosed in quotes because it contains commas)
```xaml
<Label Rotation="{z:Bind 'Lerp(0, 360, rotationPercent / 100.0)'}" ...
```
First you will need a reference to the default ExpressionParser
```csharp
var ep = ExpressionParserFactory.GetExpressionParser();
```
Then _register_ a _function_ that takes 3 parameters:
```csharp
ep.RegisterFunction("Lerp", DoLerp, 3);
```
Finally write the `DoLerp` method referenced above, with the following signature:
```csharp
private static void DoLerp(Stack<IOperand> operandStack, IBackingStore backingStore, long paramCount)
{
    // Pop the correct number of parameters from the operands stack, ** in reverse order **
    // If an operand is a variable, it is resolved from the backing store provided
    IOperand third = OperatorActions.PopAndResolve(operandStack, backingStore);
    IOperand second = OperatorActions.PopAndResolve(operandStack, backingStore);
    IOperand first = OperatorActions.PopAndResolve(operandStack, backingStore);

    double a = (double)first.GetValue();
    double b = (double)second.GetValue();
    double t = (double)third.GetValue();

    // The result is of type double
    double result = a + t * (b - a);

    // Push the result back onto the operand stack
    operandStack.Push(new Operand(-1, OperandType.Double, result));
}
```

### Aliases
Get a reference to the default ExpressionParser:
```csharp
var ep = ExpressionParserFactory.GetExpressionParser();
```
Then register a new `operator` and use the existing `matrix` for `&&`

(See the `ExpressionParserZero` [source and documentation](https://github.com/Keflon/FunctionZero.ExpressionParserZero) for more details)
```csharp
ep.RegisterOperator("AND", 4, LogicalAndMatrix.Create());
```
### Overloads
Suppose you want to add a `long` to a `string`, yielding a result of type `string`

Get a reference to the default ExpressionParser:
```csharp
var ep = ExpressionParserFactory.GetExpressionParser();
```
Then simply register the overload like this
```csharp
// Overload that will allow a long to be appended to a string
// To add a string to a long you'll need to add another overload
parser.RegisterOverload("+", OperandType.String, OperandType.Long, 
    (left, right) => new Operand(OperandType.String, (string)left.GetValue() + ((long)right.GetValue()).ToString()));
```
and to add a `string` to a `long`:
```csharp
// Overload that will allow a string to be appended to a long
// To add a long to a string you'll need to add another overload
parser.RegisterOverload("+", OperandType.Long, OperandType.String, 
    (left, right) => new Operand(OperandType.String, (long)left.GetValue() + ((string)right.GetValue()).ToString()));
```

Putting the above into action, you can then start to really have some fun
```xaml
<Label 
    Text="{z:Bind 'Player 1 score ' + playerOne.Score + 'points'}"
    Rotation="{z:Bind Lerp(0, 360, rotationPercent / 100.0)}"
/>
```
If that's not enough, you can entirely replace the `ExpressionParser` with your own by calling 
`ExpressionParserFactory.ReplaceDefaultExpressionParser(..)` This is how `z:Bind` created the 
default parser **before these registrations were moved to 
[ExpressionParserZero](https://github.com/Keflon/FunctionZero.ExpressionParserZero)** and can be used as a 
guide to creating your own:
```csharp
var ep = new ExpressionParser();

ep.RegisterFunction("Sin", DoSin, 1, 1);
ep.RegisterFunction("Cos", DoCos, 1, 1);
ep.RegisterFunction("Tan", DoTan, 1, 1);
...

ep.RegisterOperator("AND", 4, LogicalAndMatrix.Create());
ep.RegisterOperator("OR", 4, LogicalOrMatrix.Create());
ep.RegisterOperator("LT", 4, LessThanMatrix.Create());
ep.RegisterOperator("LTE", 4, LessThanOrEqualMatrix.Create());
ep.RegisterOperator("GT", 4, GreaterThanMatrix.Create());
ep.RegisterOperator("GTE", 4, GreaterThanOrEqualMatrix.Create());
ep.RegisterOperator("BAND", 4, BitwiseAndMatrix.Create());
ep.RegisterOperator("BOR", 4, BitwiseOrMatrix.Create());

ReplaceDefaultExpressionParser(ep, false);
```
Here are the methods referenced above:
```csharp
private static void DoSin(Stack<IOperand> stack, IBackingStore store, long paramCount)
{
    IOperand first = OperatorActions.PopAndResolve(stack, store);
    double val = (double)first.GetValue();
    var result = Math.Sin(val);
    stack.Push(new Operand(-1, OperandType.Double, result));
}

private static void DoCos(Stack<IOperand> stack, IBackingStore store, long paramCount)
{
    IOperand first = OperatorActions.PopAndResolve(stack, store);
    double val = (double)first.GetValue();
    var result = Math.Cos(val);
    stack.Push(new Operand(-1, OperandType.Double, result));
}

private static void DoTan(Stack<IOperand> stack, IBackingStore store, long paramCount)
{
    IOperand first = OperatorActions.PopAndResolve(stack, store);
    double val = (double)first.GetValue();
    var result = Math.Tan(val);
    stack.Push(new Operand(-1, OperandType.Double, result));
}
```

## Even more ...

### Assignment operator:
Set the property `Sample.Value`
```xaml
<Label Text="{z:Bind 'Sample.Value = Interest * Complexity'}" />
```
### Comma operator
Evaluate multiple expressions, provide the result of the last
```xaml
<Label Text="{z:Bind 'Test.Value = Count, Count * 2'}" />
```
### Caution
```xaml
<!--    This will (of course) lock up the main thread, 
        because it is re-evaluated every time `a` changes -->
<Label Text={z:Bind a = a + 1} />
```
## Notes
Expressions are reevaluated only when one of the `properties` they reference change.
