using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace FunctionZero.zBind.f
{
    public class Bind
    {
        private static char[] _dot = new[] { '.' };

        private readonly PropertyInfo _propertyInfo;
        private readonly Bind _bindingRoot;
        private readonly bool _isLeaf;
        private object _host;
        private readonly string[] _bits;
        private readonly int _currentIndex;
        private string _propertyName;
        private Bind _child;

        private object _partValue;
        public object Value { get; set; }

        public Bind(object host, string qualifiedName) : this(null, host, qualifiedName.Split(_dot), 0) { }

        protected Bind(Bind bindingRoot, object host, string[] bits, int currentIndex)
        {
            _bindingRoot = bindingRoot ?? this;
            _host = host;
            _bits = bits;
            _currentIndex = currentIndex;
            _propertyName = _bits[currentIndex];

            // Get info for the property
            _propertyInfo = host.GetType().GetProperty(_propertyName, BindingFlags.Public | BindingFlags.Instance);

            // Bail out if the property doesn't exist or cannot be read.
            if (_propertyInfo == null || _propertyInfo.CanRead == false)
                return;

            // If the value changes, respond accordingly
            if (_host is INotifyPropertyChanged inpc)
                inpc.PropertyChanged += HostPropertyChanged;

            // Refresh the value of the property
            _partValue = _propertyInfo.GetValue(_host);

            _isLeaf = _currentIndex >= _bits.Length - 1;
            if ((_isLeaf == false) && (_partValue != null))
                _child = new Bind(_bindingRoot, _partValue, _bits, _currentIndex + 1);
            else
                _bindingRoot.Value = _partValue;
        }

        // Called by parent
        private void DetachFromProperty()
        {
            if (_child != null)
                _child.DetachFromProperty();

            if (_host is INotifyPropertyChanged inpc)
                inpc.PropertyChanged -= HostPropertyChanged;
        }

        private void HostPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _propertyName)
            {
                // Our property has changed
                if (_child != null)
                    _child.DetachFromProperty();

                // Refresh the value of the property
                _partValue = _propertyInfo.GetValue(_host);

                if ((_isLeaf == false) && (_partValue != null))
                    _child = new Bind(_bindingRoot, _partValue, _bits, _currentIndex + 1);
                else
                    _bindingRoot.Value = _partValue;
            }
        }
    }
}
// One hour 21 minutes.
// Two hours 12 minutes to first 3 unit tests working (property, nested property, changing property on path)