using System;

namespace FunctionZero.yBind
{
    public class ValueChangedEventArgs : EventArgs
    {
        public ValueChangedEventArgs(object newValue)
        {
            NewValue = newValue;
        }

        public object NewValue { get; }
    }
}