using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FunctionZero.yBind
{
    internal class VariableEvaluator : IBackingStore
    {
        //private object[] _values;
        private readonly IList<string> _keys;
        private readonly IList<PathBind> _bindingCollection;

        public VariableEvaluator(IList<string> keys, IList<PathBind> bindingCollection)
        {
            _keys = keys;
            _bindingCollection = bindingCollection;
        }

        public (OperandType type, object value) GetValue(string qualifiedName)
        {
            int index = _keys.IndexOf(qualifiedName);
            object value = _bindingCollection[index].Value;

            if (value is long longResult)
                return (OperandType.Long, longResult);

            if (value is int intResult)
                return (OperandType.Long, (long)intResult);

            if (value is double doubleResult)
                return (OperandType.Double, doubleResult);

            if (value is float floatResult)
                return (OperandType.Double, (double)floatResult);

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
            //var host = _bindingExtension.Source ?? _bindingExtension.BindableTarget.BindingContext;
            //if (host != null)
            //{
            //    var bits = qualifiedName.Split(_dot);

            //    for (int c = 0; c < bits.Length - 1; c++)
            //    {
            //        PropertyInfo prop = host.GetType().GetProperty(bits[c], BindingFlags.Public | BindingFlags.Instance);
            //        if (null != prop && prop.CanRead)
            //        {
            //            host = prop.GetValue(host);
            //        }
            //        else
            //            return;
            //    }
            //    var variableName = bits[bits.Length - 1];

            //    PropertyInfo prop2 = host.GetType().GetProperty(variableName, BindingFlags.Public | BindingFlags.Instance);
            //    if (null != prop2 && prop2.CanWrite)
            //    {
            //        prop2.SetValue(host, value, null);
            //    }
            //}
        }
    }
}