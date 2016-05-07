using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class DecimalEqualityComparer : IEqualityWrapper
    {
        private ICloseEnoughConfiguration _configuration;

        public DecimalEqualityComparer(ICloseEnoughConfiguration configuration)
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
                decimal leftDouble;
                decimal rightDouble;

                if (allowTypeConversion)
                {
                    leftDouble = (decimal)Convert.ChangeType(left, typeof(decimal));
                    rightDouble = (decimal)Convert.ChangeType(right, typeof(decimal));
                }
                else
                {
                    leftDouble = (decimal)left;
                    rightDouble = (decimal)right;
                }

                return Math.Abs(leftDouble - rightDouble) <= _configuration.DecimalEpsilon(property);
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
