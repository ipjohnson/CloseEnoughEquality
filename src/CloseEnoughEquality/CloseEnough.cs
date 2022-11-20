using CloseEnoughEquality.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public static class CloseEnough
    {
        public static IEqualityComparer<T> Comparer<T>(Action<ICloseEnoughConfigurationSyntax<T>> configuration = null)
        {
            return new CloseEnough<T, T>(configuration ?? (c => { }));
        }
        
        public static void MustEqual<TLeft, TRight>(TLeft left, TRight right)
        {
            new CloseEnough<TLeft, TRight>(c => c.ThrowsExceptionIfDiscrepancies(true)).Equals(left, right);
        }

        public static void MustEqual<TLeft, TRight>(TLeft left, TRight right, Action<ICloseEnoughConfigurationSyntax<TLeft>> configuration)
        {           
            new CloseEnough<TLeft, TRight>(c => { c.ThrowsExceptionIfDiscrepancies(true); configuration?.Invoke(c); }).Equals(left, right);
        }

        public static IReadOnlyList<ICloseEnoughDiscrepancy> GetDiscrepancies<TLeft, TRight>(TLeft left, TRight right)
        {
            return new CloseEnough<TLeft, TRight>().GetDiscrepancies(left, right);
        }

        public static IReadOnlyList<ICloseEnoughDiscrepancy> GetDiscrepancies<TLeft, TRight>(TLeft left, TRight right, Action<ICloseEnoughConfigurationSyntax<TLeft>> configuration)
        {
            return new CloseEnough<TLeft, TRight>(configuration).GetDiscrepancies(left, right);
        }

        public static bool Equals<TLeft, TRight>(TLeft left, TRight right)
        {
            return new CloseEnough<TLeft, TRight>().Equals(left, right);
        }

        public static bool Equals<TLeft, TRight>(TLeft left, TRight right, Action<ICloseEnoughConfigurationSyntax<TLeft>> configuration)
        {
            return new CloseEnough<TLeft, TRight>(configuration).Equals(left, right);
        }
    }

    public class CloseEnough<TLeft, TRight> : IEqualityComparer<TLeft>
    {
        private ICloseEnoughConfiguration _configuration;

        public CloseEnough(Action<ICloseEnoughConfigurationSyntax<TLeft>> configuration = null)
        {
            _configuration = new CloseEnoughConfiguration();

            configuration?.Invoke(new CloseEnoughConfigurationSyntax<TLeft>(_configuration));
        }

        public virtual bool Equals(TLeft x, TLeft y)
        {
            return InternalEquals(x, y);
        }

        public virtual int GetHashCode(TLeft obj)
        {
            return obj.GetHashCode();
        }

        public virtual bool Equals(TLeft left, TRight right)
        {
            return InternalEquals(left, right);
        }

        public virtual IReadOnlyList<ICloseEnoughDiscrepancy> GetDiscrepancies(object left, object right)
        {
            _configuration.PushCurrentPath(typeof(TLeft).Name);
            
            if (left == null)
            {
                if (right == null)
                {
                    return _configuration.GetDiscrepenacies();
                }

                _configuration.AddDiscrepancy(left, right);

            }
            else if (right == null)
            {
                _configuration.AddDiscrepancy(left, right);
            }
            else
            {
                var wrapper = _configuration.GetEqualityWrapper(typeof(TLeft));

                if(!wrapper.Equals(left, right, new RootPropertyInfo(left)) && 
                   !wrapper.GeneratesDiscrepancy)
                {
                    _configuration.AddDiscrepancy(left, right);
                }
            }

            var discrepancy = _configuration.GetDiscrepenacies();

            if (discrepancy.Count > _configuration.AllowedDiscrepancies && 
                _configuration.ThrowsException)
            {
                throw new DiscrepancyException(typeof(TLeft), discrepancy);
            }

            return discrepancy;
        }
        
        protected virtual bool InternalEquals(object left, object right)
        {
            bool returnValue = false;
            var discrepancy = GetDiscrepancies(left, right);

            if (discrepancy.Count <= _configuration.AllowedDiscrepancies)
            {
                returnValue = true;
            }

            return returnValue;
        }
    }
}
