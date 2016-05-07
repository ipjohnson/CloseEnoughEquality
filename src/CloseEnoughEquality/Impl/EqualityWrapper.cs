using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public interface IEqualityWrapper
    {
        bool GeneratesDiscrepancy { get; }

        bool Applies(IPropertyInfo property);

        bool Equals(object left, object right, IPropertyInfo property);
    }

    public class EqualityWrapper<T> : IEqualityWrapper
    {
        private IEqualityComparer<T> _comparer;
        private Func<IPropertyInfo, bool> _filter;

        public EqualityWrapper(IEqualityComparer<T> comparer, Func<IPropertyInfo, bool> filter)
        {
            _comparer = comparer;
            _filter = filter;
        }

        public bool GeneratesDiscrepancy { get { return false; } }

        public bool Applies(IPropertyInfo property)
        {
            if(_filter != null)
            {
                return _filter(property);
            }

            return true;
        }

        public new bool Equals(object left, object right, IPropertyInfo property)
        {
            return _comparer.Equals((T)left, (T)right);
        }
    }
}
