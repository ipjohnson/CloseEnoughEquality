using CloseEnoughEquality.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public interface ICloseEnoughConfiguration
    {
        bool GenerateDiscrepancy { get; set; }

        void AllowTypeConversion(bool allow, Func<IPropertyInfo, bool> filter);

        void StringCaseSensitive(bool value, Func<IPropertyInfo, bool> filter);

        void DateTimeComparisonMode(DateTimeComparisonMode mode, Func<IPropertyInfo, bool> filter);

        DateTimeComparisonMode DateTimeComparisonMode(IPropertyInfo property);

        void FloatEpsilon(float epsilon, Func<IPropertyInfo, bool> filter = null);

        void DoubleEpsilon(double epsilon, Func<IPropertyInfo, bool> filter = null);

        void DecimalEpsilon(decimal epsilon, Func<IPropertyInfo, bool> filter = null);

        void SkipProperty(PropertyInfo property);

        void SkipPropertiesOfType(Type type, Func<IPropertyInfo, bool> filter = null);

        void UseComparer<TType>(IComparer<TType> comparer, Func<IPropertyInfo, bool> filter = null);

        void UseComparer<TType>(IEqualityComparer<TType> comparer, Func<IPropertyInfo, bool> filter = null);

        void UseCustomEquals(bool value = true, Func<IPropertyInfo, bool> filter = null);

        bool UseCustomEquals(IPropertyInfo property);

        IEqualityWrapper GetEqualityWrapper(Type t, IPropertyInfo property = null);

        bool AllowTypeConversion(IPropertyInfo property);

        bool StringCaseSensitive(IPropertyInfo property);

        float FloatEpsilon(IPropertyInfo property);

        double DoubleEpsilon(IPropertyInfo property);

        decimal DecimalEpsilon(IPropertyInfo property);

        bool ShouldSkipProperty(IPropertyInfo property);

        void PushCurrentPath(string path);

        void PopCurrentPath();

        string CurrentPath { get; }

        void AddDiscrepancy(object leftValue, object rightValue);

        void AddMissingPropertyDiscrepancy(string path);

        void AddExtraPropertyDiscrepancy(string path);

        void AddDiscrepancy(ICloseEnoughDiscrepancy discrepancy);

        int AllowedDiscrepancies { get; set; }

        IReadOnlyList<ICloseEnoughDiscrepancy> GetDiscrepenacies();

        bool ThrowsException { get; set; }
        
        void IgnoreUnmatchedProperties(bool value, Func<IPropertyInfo,bool> filter);

        bool IgnoreUnmatchedProperties(IPropertyInfo propertyInfo);
    }

    public class CloseEnoughConfiguration : ICloseEnoughConfiguration
    {
        private Dictionary<Type, List<IEqualityWrapper>> _equalityWrappers = new Dictionary<Type, List<IEqualityWrapper>>();
        private ClassEqualityComparer _defaultComparer;
        private List<PropertyFilteredConfiguration<bool>> _allowTypeConversion;
        private List<PropertyFilteredConfiguration<bool>> _ignoreUnmatchedProperties;
        private List<PropertyFilteredConfiguration<bool>> _stringCaseSensitive;
        private List<PropertyFilteredConfiguration<bool>> _useCustomEquals;
        private List<PropertyFilteredConfiguration<DateTimeComparisonMode>> _dateTimeComparisonMode;
        private List<PropertyFilteredConfiguration<float>> _floatEpsilon;
        private List<PropertyFilteredConfiguration<double>> _doubleEpsilon;
        private List<PropertyFilteredConfiguration<decimal>> _decimalEpsilon;
        private List<PropertyInfo> _skipProperty;
        private List<PropertyFilteredConfiguration<Type>> _skipProperties;
        private Stack<string> _path = new Stack<string>();
        private List<ICloseEnoughDiscrepancy> _discrepancy = new List<ICloseEnoughDiscrepancy>();
        
        public string CurrentPath
        {
            get
            {
                return _path.Aggregate("", (x, s) => x + "." + s).TrimStart('.');
            }
        }

        public int AllowedDiscrepancies { get; set; }

        public bool ThrowsException { get; set; }

        public bool AllowExtraRightProperties { get; set; }

        public bool GenerateDiscrepancy { get; set; }

        public CloseEnoughConfiguration()
        {
            Initialize();
        }

        private void Initialize()
        {
            AllowedDiscrepancies = 0;
            ThrowsException = false;
            AllowExtraRightProperties = true;
            GenerateDiscrepancy = true;

            _defaultComparer = new ClassEqualityComparer(this);
            AddEqualityWrapper<double>(new DoubleEqualityComparer(this));
            AddEqualityWrapper<decimal>(new DecimalEqualityComparer(this));
            AddEqualityWrapper<float>(new FloatEqualityComparer(this));
            AddEqualityWrapper<int>(new SimpleEqualityComparer<int>(this));
            AddEqualityWrapper<uint>(new SimpleEqualityComparer<uint>(this));
            AddEqualityWrapper<byte>(new SimpleEqualityComparer<byte>(this));
            AddEqualityWrapper<sbyte>(new SimpleEqualityComparer<sbyte>(this));
            AddEqualityWrapper<long>(new SimpleEqualityComparer<long>(this));
            AddEqualityWrapper<ulong>(new SimpleEqualityComparer<ulong>(this));
            AddEqualityWrapper<bool>(new SimpleEqualityComparer<bool>(this));
            AddEqualityWrapper<TimeSpan>(new SimpleEqualityComparer<TimeSpan>(this));
            AddEqualityWrapper<DateTime>(new DateTimeEqualityComparer(this));
            AddEqualityWrapper<DateTimeOffset>(new SimpleEqualityComparer<DateTimeOffset>(this));
            AddEqualityWrapper<string>(new StringEqualityComparer(this));
        }
        
        public void AllowTypeConversion(bool allow, Func<IPropertyInfo, bool> filter)
        {
            if(_allowTypeConversion == null)
            {
                _allowTypeConversion = new List<PropertyFilteredConfiguration<bool>>();
            }

            _allowTypeConversion.Add(new PropertyFilteredConfiguration<bool> { Value = allow, Filter = filter });
        }

        public void StringCaseSensitive(bool value, Func<IPropertyInfo, bool> filter)
        {
            if(_stringCaseSensitive == null)
            {
                _stringCaseSensitive = new List<PropertyFilteredConfiguration<bool>>();
            }

            _stringCaseSensitive.Add(new PropertyFilteredConfiguration<bool> { Value = value, Filter = filter });
        }

        public void FloatEpsilon(float epsilon, Func<IPropertyInfo, bool> filter = null)
        {
            if(_floatEpsilon == null)
            {
                _floatEpsilon = new List<PropertyFilteredConfiguration<float>>();
            }

            _floatEpsilon.Add(new PropertyFilteredConfiguration<float> { Value = epsilon, Filter = filter });
        }

        public void DoubleEpsilon(double epsilon, Func<IPropertyInfo, bool> filter = null)
        {
            if(_doubleEpsilon == null)
            {
                _doubleEpsilon = new List<PropertyFilteredConfiguration<double>>();
            }

            _doubleEpsilon.Add(new PropertyFilteredConfiguration<double> { Value = epsilon, Filter = filter });
        }

        public void DecimalEpsilon(decimal epsilon, Func<IPropertyInfo, bool> filter = null)
        {
            if(_decimalEpsilon == null)
            {
                _decimalEpsilon = new List<PropertyFilteredConfiguration<decimal>>();
            }

            _decimalEpsilon.Add(new PropertyFilteredConfiguration<decimal> { Value = epsilon, Filter = filter });
        }

        public void SkipProperty(PropertyInfo property)
        {
            if(_skipProperty == null)
            {
                _skipProperty = new List<PropertyInfo>();
            }

            _skipProperty.Add(property);
        }

        public void SkipPropertiesOfType(Type type, Func<IPropertyInfo, bool> filter)
        {
            if(_skipProperties == null)
            {
                _skipProperties = new List<PropertyFilteredConfiguration<Type>>();
            }

            _skipProperties.Add(new PropertyFilteredConfiguration<Type> { Value = type, Filter = filter });
        }

        public void UseComparer<TType>(IComparer<TType> comparer, Func<IPropertyInfo, bool> filter)
        {
            AddEqualityComparer(comparer, filter);
        }

        public void UseComparer<TType>(IEqualityComparer<TType> comparer, Func<IPropertyInfo, bool> filter = null)
        {
            AddEqualityComparer(comparer, filter);
        }

        public IEqualityWrapper GetEqualityWrapper(Type t, IPropertyInfo property)
        {
            List<IEqualityWrapper> wrappers;

            if(t.GetTypeInfo().IsEnum)
            {
                return new EnumEqualityComparer(this);
            }

            if(_equalityWrappers.TryGetValue(t,out wrappers))
            {
                var match = wrappers.LastOrDefault(w => w.Applies(property));

                if(match != null)
                {
                    return match;
                }
            }

            var baseType = t.GetTypeInfo().BaseType;

            if(baseType != null && baseType != typeof(object))
            {
                return GetEqualityWrapper(baseType, property);
            }              

            return _defaultComparer;
        }

        public bool AllowTypeConversion(IPropertyInfo propertyName)
        {
            if (_allowTypeConversion != null)
            {
                var allow = _allowTypeConversion.LastOrDefault(f => f.Matches(propertyName));

                if(allow != null)
                {
                    return allow.Value;
                }
            }

            return true;
        }

        public bool StringCaseSensitive(IPropertyInfo propertyName)
        {
            if(_stringCaseSensitive != null)
            {
                var allow = _stringCaseSensitive.LastOrDefault(f => f.Matches(propertyName));

                if(allow != null)
                {
                    return allow.Value;
                }
            }

            return true;
        }

        public float FloatEpsilon(IPropertyInfo propertyName)
        {
            if (_floatEpsilon != null)
            {
                var match = _floatEpsilon.LastOrDefault(f => f.Matches(propertyName));

                if (match != null)
                {
                    return match.Value;
                }
            }

            return float.Epsilon;
        }

        public double DoubleEpsilon(IPropertyInfo propertyName)
        {
            if (_doubleEpsilon != null)
            {
                var match = _doubleEpsilon.LastOrDefault(f => f.Matches(propertyName));

                if (match != null)
                {
                    return match != null ? match.Value : double.Epsilon;
                }
            }

            return double.Epsilon;
        }

        public decimal DecimalEpsilon(IPropertyInfo propertyName)
        {
            if (_decimalEpsilon != null)
            {
                var match = _decimalEpsilon.LastOrDefault(f => f.Matches(propertyName));

                if (match != null)
                {
                    return match.Value;
                }
            }

            return 0;
        }

        public bool ShouldSkipProperty(IPropertyInfo propertyInfo)
        {
            if(_skipProperty != null)
            {
                var match = _skipProperty.LastOrDefault(p => p.Name == propertyInfo.Name && p.DeclaringType == propertyInfo.DeclaringType);

                if(match != null)
                {
                    return true;
                }
            }

            if(_skipProperties != null)
            {
                var match = _skipProperties.LastOrDefault(s => CheckForMatchingProperty(s, propertyInfo));

                if(match != null)
                {
                    return true;
                }
            }

            return false;
        }

        public void PushCurrentPath(string path)
        {
            _path.Push(path);
        }

        public void PopCurrentPath()
        {
            _path.Pop();
        }

        public void AddDiscrepancy(object leftValue, object rightValue)
        {
            _discrepancy.Add(new CloseEnoughDiscrepancy( CurrentPath, leftValue, rightValue));
        }

        public IReadOnlyList<ICloseEnoughDiscrepancy> GetDiscrepenacies()
        {
            return _discrepancy;
        }
        
        private void AddEqualityWrapper<T>(IEqualityWrapper wrapper)
        {
            List<IEqualityWrapper> wrapperList;

            if (!_equalityWrappers.TryGetValue(typeof(T), out wrapperList))
            {
                wrapperList = new List<IEqualityWrapper>();
                _equalityWrappers[typeof(T)] = wrapperList;
            }

            wrapperList.Add(wrapper);
        }

        private void AddEqualityWrapper(Type type, IEqualityWrapper wrapper)
        {
            List<IEqualityWrapper> wrapperList;

            if (!_equalityWrappers.TryGetValue(type, out wrapperList))
            {
                wrapperList = new List<IEqualityWrapper>();
                _equalityWrappers[type] = wrapperList;
            }

            wrapperList.Add(wrapper);
        }

        private void AddEqualityComparer<T>(IComparer<T> comparer, Func<IPropertyInfo, bool> filter = null)
        {
            AddEqualityWrapper<T>(new GenericEqualityComparer<T>(comparer, filter));
        }

        private void AddEqualityComparer<T>(IEqualityComparer<T> comparer, Func<IPropertyInfo, bool> filter = null)
        {
            AddEqualityWrapper<T>(new EqualityWrapper<T>(comparer, filter));
        }

        public void AddMissingPropertyDiscrepancy(string path)
        {
            _discrepancy.Add(new MissingPropertyDiscrepancy(path));
        }

        public void AddExtraPropertyDiscrepancy(string path)
        {
            _discrepancy.Add(new ExtraPropertyDiscrepancy(path));
        }

        public void UseCustomEquals(bool value = true, Func<IPropertyInfo, bool> filter = null)
        {
            if(_useCustomEquals == null)
            {
                _useCustomEquals = new List<PropertyFilteredConfiguration<bool>>(); 
            }

            _useCustomEquals.Add(new PropertyFilteredConfiguration<bool> { Value = value, Filter = filter });
        }

        public bool UseCustomEquals(IPropertyInfo property)
        {
            if(_useCustomEquals != null)
            {
                var customEquals = _useCustomEquals.LastOrDefault(x => x.Matches(property));

                if(customEquals != null)
                {
                    return customEquals.Value;
                }
            }

            return true;
        }

        public void AddDiscrepancy(ICloseEnoughDiscrepancy discrepancy)
        {
            _discrepancy.Add(discrepancy);
        }

        public void IgnoreUnmatchedProperties(bool value, Func<IPropertyInfo, bool> filter)
        {
            if(_ignoreUnmatchedProperties == null)
            {
                _ignoreUnmatchedProperties = new List<PropertyFilteredConfiguration<bool>>();
            }

            _ignoreUnmatchedProperties.Add(new PropertyFilteredConfiguration<bool> { Value = value, Filter = filter });
        }

        public bool IgnoreUnmatchedProperties(IPropertyInfo propertyInfo)
        {
            if(_ignoreUnmatchedProperties != null)
            {
                var match = _ignoreUnmatchedProperties.LastOrDefault(x => x.Matches(propertyInfo));

                if(match != null)
                {
                    return match.Value;
                }
            }

            return false;
        }

        public void DateTimeComparisonMode(DateTimeComparisonMode mode, Func<IPropertyInfo, bool> filter)
        {
            if(_dateTimeComparisonMode == null)
            {
                _dateTimeComparisonMode = new List<PropertyFilteredConfiguration<DateTimeComparisonMode>>();
            }

            _dateTimeComparisonMode.Add(new PropertyFilteredConfiguration<DateTimeComparisonMode> { Value = mode, Filter = filter });
        }

        public DateTimeComparisonMode DateTimeComparisonMode(IPropertyInfo property)
        {
            if(_dateTimeComparisonMode != null)
            {
                var match = _dateTimeComparisonMode.LastOrDefault(p => p.Matches(property));

                if(match != null)
                {
                    return match.Value;
                }
            }

            return CloseEnoughEquality.DateTimeComparisonMode.Millisecond;
        }

        private bool CheckForMatchingProperty(PropertyFilteredConfiguration<Type> s, IPropertyInfo propertyInfo)
        {
            if(!s.Matches(propertyInfo))
            {
                return false;
            }

            var sTypeInfo = s.Value.GetTypeInfo();
            
            if(sTypeInfo.IsGenericType && sTypeInfo.IsGenericTypeDefinition)
            {
                if(sTypeInfo.IsInterface)
                {
                    var pTypeInfo = propertyInfo.PropertyType.GetTypeInfo();

                    if (pTypeInfo.IsInterface)
                    {
                        if(propertyInfo.PropertyType.IsConstructedGenericType && 
                           pTypeInfo.GetGenericTypeDefinition() == s.Value)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        foreach (var interfaceType in propertyInfo.PropertyType.GetTypeInfo().ImplementedInterfaces)
                        {
                            if (interfaceType.IsConstructedGenericType && interfaceType.GetTypeInfo().GetGenericTypeDefinition() == s.Value)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    var currentType = propertyInfo.PropertyType;

                    while(currentType != typeof(object))
                    {
                        if(currentType.IsConstructedGenericType && currentType.GetTypeInfo().GetGenericTypeDefinition() == s.Value)
                        {
                            return true;
                        }

                        currentType = currentType.GetTypeInfo().BaseType;
                    }
                }
            }
            else
            {
                return s.Value.GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType.GetTypeInfo());
            }

            return false;
        }

    }
}
