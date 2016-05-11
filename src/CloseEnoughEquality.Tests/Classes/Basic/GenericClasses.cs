using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Tests.Classes.Basic
{
    public interface IGenericInterface<T>
    {
        T TValue { get; set; }
    }

    public class ImplementationClass<T> : IGenericInterface<T>
    {
        public T TValue { get; set; }
    }

    public class GenericInterfacePropertyClass
    {
        public IGenericInterface<int> InterfaceClass { get; set; }
    }

    public class GenericImplementationPropertyClass
    {
        public ImplementationClass<int> InterfaceClass { get; set; }
    }
}
