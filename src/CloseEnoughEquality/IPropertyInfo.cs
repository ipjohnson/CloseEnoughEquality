using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public interface IPropertyInfo
    {
        PropertyAttributes Attributes { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        Type DeclaringType { get; }

        string Name { get; }

        Type PropertyType {  get;  }

        ParameterInfo[] GetIndexParameters();

        object GetValue();

        void SetValue(object value);

    }
}
