using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.MvvmZero;
using FunctionZero.zBind.z;
using FunctionZero.zBindTestApp.Mvvm.Pages;
using FunctionZero.zBindTestApp.Mvvm.PageViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FunctionZero.zBindTestApp
{
    public partial class App : Application
    {
        public App(IPageServiceZero pageService)
        {
            InitializeComponent();
            var ep = ExpressionParserFactory.GetExpressionParser();
            ep.RegisterFunction("GetColor", DoGetColor, 3, 3);
            ep.RegisterFunction("Lerp", DoLerp, 3, 3);

            MainPage = pageService.MakePage<HomePage, HomePageVm>(async (vm) => await vm.InitAsync());
        }

        private static void DoGetColor(Stack<IOperand> operandStack, IBackingStore backingStore, long paramCount)
        {
            // Pop the correct number of parameters from the operands stack, ** in reverse order **
            // If an operand is a variable, it is resolved from the backing store provided
            IOperand fourth = OperatorActions.PopAndResolve(operandStack, backingStore);
            IOperand third = OperatorActions.PopAndResolve(operandStack, backingStore);
            IOperand second = OperatorActions.PopAndResolve(operandStack, backingStore);
            IOperand first = OperatorActions.PopAndResolve(operandStack, backingStore);

            double r = (double)first.GetValue();
            double g = (double)second.GetValue();
            double b = (double)third.GetValue();
            double a = (double)fourth.GetValue();

            // The result is of type Color
            object result = new Color(r, g, b, 1);

            // Push the result back onto the operand stack
            operandStack.Push(new Operand(-1, OperandType.Double, result));
        }

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
