using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class GenericEqualityComparer<T> : IEqualityWrapper
    {
        private IComparer<T> _comparer;
        private Func<IPropertyInfo, bool> _filter;

        public GenericEqualityComparer(IComparer<T> comparer, Func<IPropertyInfo, bool> filter)
        {
            _comparer = comparer;
            _filter = filter;
        }

        public bool GeneratesDiscrepancy { get { return false; } }

        public bool Applies(IPropertyInfo propertyName)
        {
            if (_filter != null)
            {
                return _filter(propertyName);
            }

            return true;
        }

        public new bool Equals(object left, object right, IPropertyInfo property)
        {
            return _comparer.Compare((T)left, (T)right) == 0;
        }
    }
}
