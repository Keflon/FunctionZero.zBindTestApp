using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace FunctionZero.zBind.z
{
    internal class VariableEvaluator : IBackingStore
    {
        private object[] _values;
        private readonly IList<string> _keys;
        private readonly Bind _bindingExtension;

        public VariableEvaluator(IList<string> keys, Bind bindingExtension)
        {
            _keys = keys;
            _bindingExtension = bindingExtension;
        }

        internal void SetValues(object[] values)
        {
            _values = values;
        }

        public (OperandType type, object value) GetValue(string qualifiedName)
        {
            int index = _keys.IndexOf(qualifiedName);
            object value = _values[index];

            if (value is long longResult)
                return (OperandType.Long, longResult);

            if (value is int intResult)
                return (OperandType.Long, intResult);

            if (value is double doubleResult)
                return (OperandType.Double, doubleResult);

            if (value is float floatResult)
                return (OperandType.Double, floatResult);

            if (value is bool boolResult)
                return (OperandType.Bool, boolResult);

            if (value is string stringResult)
                return (OperandType.String, stringResult);

            if (value == null)
                return (OperandType.Null, null);

            return (OperandType.Object, value);
        }

        private static char[] _dot = new[] { '.' };

        public void SetValue(string qualifiedName, object value)
        {
            var host = _bindingExtension.Source ?? _bindingExtension.BindableTarget.BindingContext;
            if (host != null)
            {
                var bits = qualifiedName.Split(_dot);

                for (int c = 0; c < bits.Length - 1; c++)
                {
                    PropertyInfo prop = host.GetType().GetProperty(bits[c], BindingFlags.Public | BindingFlags.Instance);
                    if (null != prop && prop.CanRead)
                    {
                        host = prop.GetValue(host);
                    }
                    else
                        return;
                }
                var variableName = bits[bits.Length - 1];

                PropertyInfo prop2 = host.GetType().GetProperty(variableName, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop2 && prop2.CanWrite)
                {
                    prop2.SetValue(host, value, null);
                }
            }
        }
    }
}