using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FunctionZero.zBind.z
{
    [ContentProperty("Expression")]
    public class Bind : IMarkupExtension<BindingBase>
    {
        public string Expression { set; get; }
        public BindingMode Mode { get; set; }

        private IList<string> _bindingLookup;

        public object Source { get; set; }

        public Bind()
        {
        }

        private MultiBinding _multiBind;

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            _bindingLookup = new List<string>();

            if (string.IsNullOrEmpty(Expression))
            {
                IXmlLineInfo lineInfo = serviceProvider.GetService(typeof(IXmlLineInfoProvider)) is IXmlLineInfoProvider lineInfoProvider ? lineInfoProvider.XmlLineInfo : new XmlLineInfo();
                throw new XamlParseException("ZeroBind requires 'Expression' property to be set", lineInfo);
            }

            IProvideValueTarget pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            BindableObject bindableTarget = pvt.TargetObject as BindableObject;
            //BindableProperty bindableProperty = pvt.TargetProperty as BindableProperty;

            //bindableTarget.BindingContextChanged += TargetOnBindingContextChanged;
            //bindableTarget.SetValue(bindableProperty, 32);

            var bindingSourceObject = Source ?? bindableTarget.BindingContext;

            var ep = ExpressionParserFactory.GetExpressionParser();

            _multiBind = new MultiBinding();

            try
            {
                var compiledExpression = ep.Parse(Expression);
                foreach (IToken item in compiledExpression)
                {
                    if (item is Operand op)
                    {
                        if (op.Type == OperandType.Variable)
                        {
                            if (_bindingLookup.Contains(op.ToString()) == false)
                            {
                                var binding = new Binding(op.ToString(), BindingMode.OneWay, null, null, null, bindingSourceObject);
                                _bindingLookup.Add(op.ToString());
                                _multiBind.Bindings.Add(binding);
                            }
                        }
                    }
                }
                _multiBind.Converter = new EvaluatorMultiConverter(_bindingLookup, compiledExpression);
            }
            catch (ExpressionParserException ex)
            {
                IXmlLineInfo lineInfo = serviceProvider.GetService(typeof(IXmlLineInfoProvider)) is IXmlLineInfoProvider lineInfoProvider ? lineInfoProvider.XmlLineInfo : new XmlLineInfo();
                Debug.WriteLine(
                    $"z:Bind exception at line {lineInfo.LineNumber}, Column {lineInfo.LinePosition + ex.Offset}: " + Environment.NewLine +
                    $"Expression '{Expression}' error at offset {ex.Offset} - " +
                    ex.Message);
                return new Binding("_dummy_value_");
            }
            return _multiBind;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
        }
    }
}
