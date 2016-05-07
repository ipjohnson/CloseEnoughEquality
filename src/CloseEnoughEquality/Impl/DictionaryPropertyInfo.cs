using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class DictionaryPropertyInfo<TKey,TValue> : IPropertyInfo
    {
        private IDictionary<TKey, TValue> _dictionary;
        private TKey _key;
        private TValue _value;

        public DictionaryPropertyInfo(IDictionary<TKey, TValue> dictionary, TKey key, TValue value) 
        {
            _dictionary = dictionary;
            _key = key;
            _value = value;
        }

        public PropertyAttributes Attributes {  get { return PropertyAttributes.None; } }

        public bool CanRead {  get { return true; } }

        public bool CanWrite { get { return true; } }

        public Type DeclaringType {  get { return _dictionary.GetType(); } }

        public string Name {  get { return _key.ToString(); } }

        public Type PropertyType {  get { return _value != null ? _value.GetType() : typeof(object); } }

        public ParameterInfo[] GetIndexParameters() { return new ParameterInfo[0]; }

        public object GetValue()
        {
            return _value;
        }

        public void SetValue(object value)
        {
            TValue tValue = (TValue)value;

            _dictionary[_key] = tValue;
            _value = tValue;
        }
    }
}
