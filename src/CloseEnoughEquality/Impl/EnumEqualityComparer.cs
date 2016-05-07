using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class EnumEqualityComparer : IEqualityWrapper
    {
        private ICloseEnoughConfiguration _configuration;

        public EnumEqualityComparer(ICloseEnoughConfiguration configuration)
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
                if(allowTypeConversion)
                {
                    var leftValue = Convert.ChangeType(left, property.PropertyType);
                    object rightValue;

                    if (right.GetType() == property.PropertyType)
                    {
                        rightValue = right;
                    }
                    else
                    {
                        rightValue = Enum.Parse(property.PropertyType, right.ToString());
                    }
                    
                    return leftValue.Equals(rightValue);
                }
                else
                {
                    return left.Equals(right);
                }
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
