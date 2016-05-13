using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class FieldInfoWrapper : IPropertyInfo
    {
        private FieldInfo _field;
        private object _instance;

        public FieldInfoWrapper(FieldInfo field, object instance)
        {
            _field = field;
            _instance = instance;
        }

        public PropertyAttributes Attributes {  get { return PropertyAttributes.None; } }

        public bool CanRead {  get { return true; } }

        public bool CanWrite {  get { return true; } }

        public Type DeclaringType {  get { return _field.DeclaringType; } }

        public string Name {  get { return _field.Name; } }

        public Type PropertyType {  get { return _field.FieldType; } }

        public ParameterInfo[] GetIndexParameters() { return new ParameterInfo[0]; }

        public object GetValue()
        {
            return _field.GetValue(_instance);
        }

        public void SetValue(object value)
        {
            _field.SetValue(_instance, value);
        }
    }
}
