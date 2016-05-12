using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class DoubleEqualityComparer : IEqualityWrapper
    {
        private ICloseEnoughConfiguration _configuration;

        public DoubleEqualityComparer(ICloseEnoughConfiguration configuration)
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
                double leftDouble;
                double rightDouble;

                if (allowTypeConversion)
                {
                    leftDouble = (double)Convert.ChangeType(left, typeof(double));
                    rightDouble = (double)Convert.ChangeType(right, typeof(double));
                }
                else
                {
                    leftDouble = (double)left;
                    rightDouble = (double)right;
                }

                return Math.Abs(leftDouble - rightDouble) <= _configuration.DoubleEpsilon(property);
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
