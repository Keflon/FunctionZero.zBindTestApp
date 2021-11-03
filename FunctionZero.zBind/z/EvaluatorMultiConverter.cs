using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Evaluator;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace FunctionZero.zBind.z
{
    internal class EvaluatorMultiConverter : IMultiValueConverter
    {
        private readonly VariableEvaluator _evaluator;
        private readonly ExpressionTree _compiledExpressionTree;
        private readonly ExpressionTreeNode _unExpressionTreeValueParent;

        private readonly ExpressionTree _unExpressionTree;

        public EvaluatorMultiConverter(ICollection<string> keys, ExpressionTree compiledExpression, Bind bindingExtension)
        {
            _evaluator = new VariableEvaluator(new List<string>(keys), bindingExtension);
            _compiledExpressionTree = compiledExpression;

            ExpressionTree expression = ExpressionParserZero.Binding.ExpressionParserFactory.GetExpressionParser().Parse("a = b");
            

            // Clone the expression.
            // This ExpressionTree will lie about its RpnTokens.
            var unExpressionTree = new ExpressionTree(new List<IToken>());
            //var equalityOperator = ExpressionParserZero.Binding.ExpressionParserFactory.GetExpressionParser().GetEqualityOperator();
            ExpressionTreeNode equalityOperatorNode = expression.RootNodeList[0];
            equalityOperatorNode.Children.Clear();
            var unExpressionTreeNode = equalityOperatorNode;
            unExpressionTree.RootNodeList.Add(unExpressionTreeNode);

            var node = _compiledExpressionTree.RootNodeList[0];

            // For each unary operator in the _compiledExpressionTree, copy it to the unExpressionTree.
            while (node.Children.Count == 1)
            {
                unExpressionTreeNode.AddChild(new ExpressionTreeNode(node.Token, 1));
                unExpressionTreeNode = unExpressionTreeNode.Children[0];
                node = node.Children[0];
            }
            if (node.Token is Operand operand)
            {
                if (operand.Type == OperandType.Variable)
                {
                    // Unnecessary test?
                    if (node.Token.TokenType == TokenType.Operand)
                    {
                        // Leaf node of original expression.
                        // We'll want to substitute this with an operand wrapping the new value.
                        _unExpressionTreeValueParent = unExpressionTreeNode;
                        // Placeholder
                        _unExpressionTreeValueParent.AddChild(new ExpressionTreeNode(null, 0));
                        // node is the variable we want to assign.
                        string variableName = (string)((Operand)node.Token).GetValue();
                        //_variableType = ((Operand)node.Token).Type;
                        unExpressionTree.RootNodeList[0].Children.Add(new ExpressionTreeNode(new Operand(OperandType.Variable, variableName), 0));
                        _unExpressionTree = unExpressionTree;
                    }
                }
            }
            else
            {
                // Non-unary operator.
            }

            //if (_compiledExpression.RootNodeList.Count == 1)
            //if (_compiledExpression.RootNodeList[0].Token.TokenType == ExpressionParserZero.Tokens.TokenType.Operand)
            //    if (((Operand)_compiledExpression.RootNodeList[0].Token).Type == OperandType.Variable)
            //        _writeBack = (Operand)_compiledExpression.RootNodeList[0].Token;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            _evaluator.SetValues(values);

            try
            {
                var stack = _compiledExpressionTree.Evaluate(_evaluator);

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
            if (_unExpressionTree != null)
            {
                if (BackingStoreHelpers.OperandTypeLookup.TryGetValue(value.GetType(), out var theOperandType))
                {
                    var valueContainer = new Operand(theOperandType, value);
                    _unExpressionTreeValueParent.Children[0] = new ExpressionTreeNode(valueContainer, 0);
                    _unExpressionTree.Evaluate(_evaluator);
                }
            }
            else
            {
                Debug.WriteLine("z:Bind attempt to write a value back to an expression. This is only possible if the expression contains one and only one token and the token is a variable.");
            }
            return null;


            //if(_writeBack != null)
            //{
            //    _evaluator.SetValue(_writeBack.GetValue().ToString(), value);
            //}
            //else
            //{
            //    Debug.WriteLine("z:Bind attempt to write a value back to an expression. This is only possible if the expression contains one and only one token and the token is a variable.");
            //}
        }
    }
}