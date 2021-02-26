using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Parser.FunctionMatrices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.zBind.z
{
    public class ExpressionParserFactory
    {
        private static ExpressionParser _expressionParser;

        public static ExpressionParser GetExpressionParser()
        {
            if (_expressionParser == null)
            {
                var ep = new ExpressionParser();

                ep.RegisterFunction("Sin", DoSin, 1, 1);
                ep.RegisterFunction("Cos", DoCos, 1, 1);
                ep.RegisterFunction("Tan", DoTan, 1, 1);

                ep.RegisterOperator("AND", 4, LogicalAndMatrix.Create());
                ep.RegisterOperator("OR", 4, LogicalOrMatrix.Create());
                ep.RegisterOperator("LT", 4, LessThanMatrix.Create());
                ep.RegisterOperator("LTE", 4, LessThanOrEqualMatrix.Create());
                ep.RegisterOperator("GT", 4, GreaterThanMatrix.Create());
                ep.RegisterOperator("GTE", 4, GreaterThanOrEqualMatrix.Create());
                ep.RegisterOperator("BAND", 4, BitwiseAndMatrix.Create());
                ep.RegisterOperator("BOR", 4, BitwiseOrMatrix.Create());

                ReplaceDefaultExpressionParser(ep, false);
            }
            return _expressionParser;
        }

        public static void ReplaceDefaultExpressionParser(ExpressionParser replacement, bool allowLateReplacement)
        {
            if (_expressionParser == null || allowLateReplacement)
            {
                _expressionParser = replacement;
            }
            else
            {
                throw new InvalidOperationException("Attempt to replace default ExpressionParser after it has created been created for a consumer");
            }
        }

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
    }
}
