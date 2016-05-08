using CloseEnoughEquality.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public enum DateTimeComparisonMode
    {
        Exact,
        Millisecond,
        Second,
        Minute,
        Hour,
        Day,
        Month,
        Year
    }

    public interface ICloseEnoughConfigurationSyntax<T>
    {
        /// <summary>
        /// Number of discrepancies allowed to still be equal
        /// </summary>
        /// <param name="numberOfDiscrepancy"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> AllowedDiscrepancies(int numberOfDiscrepancy = 0);
        
        /// <summary>
        /// Allow for type conversion to compare, true by default
        /// </summary>
        /// <param name="allow"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> AllowTypeConversion(bool allow = true, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Should string compare be case sensitive, by default yes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> StringCaseSensitive(bool value = true, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Float epsilon, used when comparing floats
        /// </summary>
        /// <param name="epsilon"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> FloatEpsilon(float epsilon = float.Epsilon, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// DateTime Comparison Mode
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> DateTimeComparisonMode(DateTimeComparisonMode mode = CloseEnoughEquality.DateTimeComparisonMode.Millisecond, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Double epsilon, used when comparing doubles
        /// </summary>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> DoubleEpsilon(double epsilon = double.Epsilon, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Decimal epsilon, used when comparing decimals, by default 0
        /// </summary>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> DecimalEpsilon(decimal epsilon = 0, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Ignore properties on the left object that don't exist on the right
        /// </summary>
        /// <param name="value"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> IgnoreUnmatchedProperties(bool value = true, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Skip a specified property when comparing
        /// </summary>
        /// <param name="skipProperty">property to skip</param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> SkipProperty(Expression<Func<T,object>> skipProperty);

        /// <summary>
        /// Skip a specific type of property when comparing
        /// </summary>
        /// <typeparam name="TSkip">type of property</typeparam>
        /// <param name="filter">optional filter</param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> SkipPropertiesOfType<TSkip>(Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Skip a specific type of property when comparing
        /// </summary>
        /// <param name="type">type of property to skip</param>
        /// <param name="filter">optional filter</param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> SkipPropertiesOfType(Type type, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Throw Discrepancy Exception if not equal, false by default
        /// </summary>
        /// <param name="value">throws exception if true</param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> ThrowsExceptionIfDiscrepancies(bool value = false);

        /// <summary>
        /// Use specific comparer for a given type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="comparer"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> UseComparer<TType>(IComparer<TType> comparer, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Use specific equality comparer for a given type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="comparer"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> UseComparer<TType>(IEqualityComparer<TType> comparer, Func<IPropertyInfo, bool> filter = null);

        /// <summary>
        /// Use Equals method if they have been overriden from object
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="filter">filter for properties</param>
        /// <returns></returns>
        ICloseEnoughConfigurationSyntax<T> UseCustomEquals(bool value = true, Func<IPropertyInfo, bool> filter = null);
    }

    public class CloseEnoughConfigurationSyntax<T> : ICloseEnoughConfigurationSyntax<T>
    {
        private ICloseEnoughConfiguration _configuration;

        public CloseEnoughConfigurationSyntax(ICloseEnoughConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ICloseEnoughConfigurationSyntax<T> AllowedDiscrepancies(int numberOfDiscrepancy = 0)
        {
            _configuration.AllowedDiscrepancies = numberOfDiscrepancy;

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> AllowTypeConversion(bool allow = true, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.AllowTypeConversion(allow, filter);

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> DateTimeComparisonMode(DateTimeComparisonMode mode = CloseEnoughEquality.DateTimeComparisonMode.Millisecond, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.DateTimeComparisonMode(mode, filter);
            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> DecimalEpsilon(decimal epsilon = 0, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.DecimalEpsilon(epsilon, filter);

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> DoubleEpsilon(double epsilon = double.Epsilon, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.DoubleEpsilon(epsilon, filter);

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> FloatEpsilon(float epsilon = float.Epsilon, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.FloatEpsilon(epsilon, filter);

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> IgnoreUnmatchedProperties(bool value = true, Func<IPropertyInfo, bool> filter = null)
        {
            throw new NotImplementedException();
        }

        public ICloseEnoughConfigurationSyntax<T> SkipPropertiesOfType(Type type, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.SkipPropertiesOfType(type, filter);
            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> SkipPropertiesOfType<TSkip>(Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.SkipPropertiesOfType(typeof(T), filter);

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> SkipProperty(Expression<Func<T,object>> skipProperty)
        {
            MemberExpression memberExpression = skipProperty.Body as MemberExpression;
            PropertyInfo propertyInfo = null;

            if (memberExpression != null)
            {
                propertyInfo = memberExpression.Member as PropertyInfo;
            }
            if(propertyInfo == null)
            {
                throw new Exception("Linq expression cannot be used, must be property");
            }

            _configuration.ShouldSkipProperty(new PropertyInfoWrapper( propertyInfo, null));

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> StringCaseSensitive(bool value = true, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.StringCaseSensitive(value, filter);

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> ThrowsExceptionIfDiscrepancies(bool value = false)
        {
            _configuration.ThrowsException = value;

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> UseComparer<TType>(IEqualityComparer<TType> comparer, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.UseComparer(comparer,filter);

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> UseComparer<TType>(IComparer<TType> comparer, Func<IPropertyInfo, bool> filter = null)
        {
            _configuration.UseComparer(comparer, filter);

            return this;
        }

        public ICloseEnoughConfigurationSyntax<T> UseCustomEquals(bool value = true, Func<IPropertyInfo, bool> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
