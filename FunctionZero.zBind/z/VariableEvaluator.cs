using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FunctionZero.zBind.z
{
    internal class VariableEvaluator : IBackingStore
    {
        private object[] _values;
        private readonly IList<string> _keys;

        public VariableEvaluator(IList<string> keys)
        {
            _keys = keys;
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

        public void SetValue(string qualifiedName, object value)
        {
            throw new NotImplementedException();
        }
    }
}