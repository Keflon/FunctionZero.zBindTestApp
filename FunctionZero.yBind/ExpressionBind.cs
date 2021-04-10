using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.yBind
{
    public class ExpressionBind
    {
        private readonly object _host;
        private readonly IList<string> _bindingLookup;
        private readonly IList<PathBind> _bindingCollection;
        private readonly VariableEvaluator _evaluator;
        private readonly ExpressionParserZero.Parser.TokenList _compiledExpression;
        private bool _isStale;

        public bool IsStale
        {
            get => _isStale;
            set
            {
                if (value != _isStale)
                {
                    _isStale = value;
                    if (value == true)
                        ValueIsStale?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private object _value;

        public object Value
        {
            get => _value;
            set
            {
                IsStale = false;
                if (value != _value)
                {
                    _value = value;
                    ValueChanged?.Invoke(this, new ValueChangedEventArgs(value));
                }
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public event EventHandler<EventArgs> ValueIsStale;

        public ExpressionBind(object host, string expression)
        {
            _host = host;
            _bindingLookup = new List<string>();
            _bindingCollection = new List<PathBind>();

            var ep = ExpressionParserFactory.GetExpressionParser();

            _compiledExpression = ep.Parse(expression);

            foreach (IToken item in _compiledExpression)
            {
                if (item is Operand op)
                {
                    if (op.Type == OperandType.Variable)
                    {
                        if (_bindingLookup.Contains(op.ToString()) == false)
                        {
                            var binding = new PathBind(host, op.ToString(), SomethingChanged);
                            _bindingLookup.Add(op.ToString());
                            _bindingCollection.Add(binding);
                        }
                    }
                }
            }
            _evaluator = new VariableEvaluator(_bindingLookup, _bindingCollection);

            Evaluate();
        }

        public object Evaluate()
        {
            if (IsStale == true)
            {
                try
                {
                    var stack = _compiledExpression.Evaluate(_evaluator);
                    var operand = stack.Pop();

                    if (operand.Type == OperandType.Variable)
                    {
                        var valueAndType = _evaluator.GetValue((string)operand.GetValue());
                        Value = valueAndType.value;
                    }
                    Value = operand.GetValue();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return Value;
        }

        private void SomethingChanged(object newValue)
        {
            IsStale = true;
        }
    }
}
