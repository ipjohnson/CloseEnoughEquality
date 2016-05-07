using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class SimpleEqualityComparer<T> : IEqualityWrapper where T : IComparable
    {
        private ICloseEnoughConfiguration _configuration;

        public SimpleEqualityComparer(ICloseEnoughConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool GeneratesDiscrepancy { get { return false; } }

        public bool Applies(IPropertyInfo property)
        {
            return true;
        }

        public bool Equals(object left, object right, IPropertyInfo property)
        {
            bool allowTypeConversion = _configuration.AllowTypeConversion(property);

            try
            {
                T leftValue;
                T rightValue;

                if (allowTypeConversion)
                {
                    leftValue = (T)Convert.ChangeType(left, typeof(T));
                    rightValue = (T)Convert.ChangeType(right, typeof(T));
                }
                else
                {
                    leftValue = (T)left;
                    rightValue = (T)right;
                }

                return Comparer<T>.Default.Compare(leftValue,rightValue) == 0;
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
