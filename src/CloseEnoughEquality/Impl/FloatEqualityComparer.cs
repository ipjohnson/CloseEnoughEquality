using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class FloatEqualityComparer : IEqualityWrapper
    {
        private ICloseEnoughConfiguration _configuration;

        public FloatEqualityComparer(ICloseEnoughConfiguration configuration)
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
                float leftDouble;
                float rightDouble;

                if(allowTypeConversion)
                {
                    leftDouble = (float)Convert.ChangeType(left, typeof(float));
                    rightDouble = (float)Convert.ChangeType(right, typeof(float));
                }
                else
                {
                    leftDouble = (float)left;
                    rightDouble = (float)right;
                }
                
                return Math.Abs(leftDouble - rightDouble) <= _configuration.FloatEpsilon(property);
            }
            catch(Exception)
            {

            }

            return false;
        }
    }
}
