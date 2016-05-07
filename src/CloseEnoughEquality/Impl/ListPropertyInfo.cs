using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class ListPropertyInfo : IPropertyInfo
    {
        private Type _listType;
        private string _index;
        private object _value;

        public ListPropertyInfo(Type listType, string index, object value)
        {
            _listType = listType;
            _index = index;
            _value = value;
        }

        public PropertyAttributes Attributes {  get { return PropertyAttributes.None; } }

        public bool CanRead {  get { return true; } }

        public bool CanWrite {  get { return false; } }

        public Type DeclaringType {  get { return _listType; } }

        public string Name {  get { return _index; } }

        public Type PropertyType {  get { return _value.GetType(); } }

        public ParameterInfo[] GetIndexParameters() { return new ParameterInfo[0]; }

        public object GetValue()
        {
            return _value;
        }

        public void SetValue(object value)
        {
            throw new NotSupportedException("Not supported on this type");
        }
    }
}
