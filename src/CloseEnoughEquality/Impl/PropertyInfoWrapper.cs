using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class PropertyInfoWrapper : IPropertyInfo
    {
        private PropertyInfo _propertyInfo;
        private object _instance;

        public PropertyInfoWrapper(PropertyInfo property, object instance)
        {
            _propertyInfo = property;
            _instance = instance;
        }

        public PropertyAttributes Attributes {  get { return _propertyInfo.Attributes; } }

        public bool CanRead {  get { return _propertyInfo.CanRead; } }

        public bool CanWrite {  get { return _propertyInfo.CanWrite; } }

        public Type DeclaringType {  get { return _propertyInfo.DeclaringType; } }

        public string Name {  get { return _propertyInfo.Name; } }

        public Type PropertyType {  get { return _propertyInfo.PropertyType; } }

        public ParameterInfo[] GetIndexParameters() { return _propertyInfo.GetIndexParameters(); }

        public object GetValue()
        {
            return _propertyInfo.GetValue(_instance);
        }

        public void SetValue(object value)
        {
            _propertyInfo.SetValue(_instance, value);
        }
    }
}
