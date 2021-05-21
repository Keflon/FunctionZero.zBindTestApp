using FunctionZero.ExpressionParserZero.Evaluator;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace FunctionZero.zBind.z
{
    internal class EvaluatorMultiConverter : IMultiValueConverter
    {
        private readonly VariableEvaluator _evaluator;
        private readonly ExpressionTree _compiledExpression;

        public EvaluatorMultiConverter(ICollection<string> keys, ExpressionTree compiledExpression, Bind bindingExtension)
        {
            _evaluator = new VariableEvaluator(new List<string>(keys), bindingExtension);
            _compiledExpression = compiledExpression;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            _evaluator.SetValues(values);

            try
            {
                var stack = _compiledExpression.Evaluate(_evaluator);

                var operand = stack.Pop();

                if (operand.Type == OperandType.Variable)
                {
                    var valueAndType = _evaluator.GetValue((string)operand.GetValue());

                    return valueAndType.value;
                }
                return operand.GetValue();

            }
            catch (Exception ex)
            {
                if (targetType.IsValueType && Nullable.GetUnderlyingType(targetType) == null)
                    return Activator.CreateInstance(targetType);
                else
                    return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}