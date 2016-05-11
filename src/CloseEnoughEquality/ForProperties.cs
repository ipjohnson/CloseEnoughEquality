using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public class PropertiesFilter
    {
        private List<Func<IPropertyInfo, bool>> _filter = new List<Func<IPropertyInfo, bool>>();
        private bool? _orLogic;

        public PropertiesFilter OfType<T>()
        {
            _filter.Add(p => typeof(T).GetTypeInfo().IsAssignableFrom(p.PropertyType.GetTypeInfo()));

            return this;
        }

        public PropertiesFilter OfType(Type t)
        {
            _filter.Add(p => t.GetTypeInfo().IsAssignableFrom(p.PropertyType.GetTypeInfo()));

            return this;
        }

        public PropertiesFilter Named(string name)
        {
            _filter.Add(p => p.Name == name);

            return this;
        }

        public PropertiesFilter EndsWith(string postfix)
        {
            _filter.Add(p => p.Name.EndsWith(postfix));

            return this;
        }

        public PropertiesFilter StartsWith(string prefix)
        {
            _filter.Add(p => p.Name.StartsWith(prefix));

            return this;
        }

        public PropertiesFilter DeclaringType<T>()
        {
            _filter.Add(p => p.DeclaringType.GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()));

            return this;
        }

        public PropertiesFilter DeclaringType(Type t)
        {
            _filter.Add(p => p.DeclaringType.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()));

            return this;
        }

        public PropertiesFilter Or
        {
            get
            {
                if (_orLogic.HasValue && !_orLogic.Value)
                {
                    throw new Exception("Cannot mix and/or logic together");
                }

                _orLogic = true;

                return this;
            }
        }

        public PropertiesFilter And
        {
            get
            {
                if(_orLogic.HasValue && _orLogic.Value)
                {
                    throw new Exception("Cannot mix and/or logic together");
                }

                _orLogic = false;

                return this;
            }
        }

        public static implicit operator Func<IPropertyInfo,bool>(PropertiesFilter filter)
        {
            return !filter._orLogic.HasValue || !filter._orLogic.Value ?
                    new Func<IPropertyInfo,bool>(filter.AndLogic) :
                    new Func<IPropertyInfo, bool>(filter.OrLogic);
        }

        private bool AndLogic(IPropertyInfo property)
        {
            foreach(var filter in _filter)
            {
                if(!filter(property))
                {
                    return false;
                }
            }

            return true;
        }

        private bool OrLogic(IPropertyInfo property)
        {
            foreach (var filter in _filter)
            {
                if (filter(property))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class ForProperties
    {
        public static PropertiesFilter OfType<T>()
        {
            return new PropertiesFilter().OfType<T>();
        }

        public static PropertiesFilter OfType(Type t)
        {
            return new PropertiesFilter().OfType(t);
        }

        public static PropertiesFilter Named(string name)
        {
            return new PropertiesFilter().Named(name);
        }

        public static PropertiesFilter EndsWith(string postfix)
        {
            return new PropertiesFilter().EndsWith(postfix);
        }

        public static PropertiesFilter StartsWith(string prefix)
        {
            return new PropertiesFilter().StartsWith(prefix);
        }

        public static PropertiesFilter DeclaringType<T>()
        {
            return new PropertiesFilter().DeclaringType<T>();
        }

        public static PropertiesFilter DeclaringType(Type t)
        {
            return new PropertiesFilter().DeclaringType(t);
        }
    }
}
