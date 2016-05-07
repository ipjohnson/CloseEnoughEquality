using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public static class ForProperties
    {
        public static Func<IPropertyInfo,bool> Named(string name)
        {
            return p => p.Name == name;
        }

        public static Func<IPropertyInfo,bool> EndsWith(string postfix)
        {
            return p => p.Name.EndsWith(postfix);
        }

        public static Func<IPropertyInfo,bool> StartsWith(string prefix)
        {
            return p => p.Name.StartsWith(prefix);
        }
    }
}
