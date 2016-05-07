using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class PropertyFilteredConfiguration<T>
    {
        public T Value { get; set; }

        public Func<IPropertyInfo, bool> Filter { get; set; }

        public bool Matches(IPropertyInfo property)
        {
            return Filter != null ? Filter(property) : true;
        }
    }
}
