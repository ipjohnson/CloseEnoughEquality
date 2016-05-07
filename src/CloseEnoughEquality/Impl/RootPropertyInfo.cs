using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class RootPropertyInfo : IPropertyInfo
    {
        private object _instance;

        public RootPropertyInfo(object instance)
        {
            _instance = instance;
        }

        public PropertyAttributes Attributes { get { return PropertyAttributes.None; } }

        public bool CanRead {  get { return true; } }

        public bool CanWrite {  get { return false; } }

        public Type DeclaringType { get { return typeof(CloseEnough); } }

        public string Name {  get { return "Root"; } }

        public Type PropertyType {  get { return _instance.GetType(); } }

        public ParameterInfo[] GetIndexParameters() { return new ParameterInfo[] { }; }

        public object GetValue()
        {
            return _instance;
        }

        public void SetValue(object value)
        {
            throw new NotSupportedException("Cannot set value");
        }
    }
}
