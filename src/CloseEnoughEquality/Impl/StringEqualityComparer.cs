using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class StringEqualityComparer : IEqualityWrapper
    {
        private ICloseEnoughConfiguration _configuration;

        public StringEqualityComparer(ICloseEnoughConfiguration configuration)
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
            bool caseInsensitive = _configuration.StringCaseSensitive(property);

            try
            {
                string leftValue;
                string rightValue;

                if (allowTypeConversion)
                {
                    leftValue = (string)Convert.ChangeType(left, typeof(string));
                    rightValue = (string)Convert.ChangeType(right, typeof(string));
                }
                else
                {
                    leftValue = (string)left;
                    rightValue = (string)right;
                }

                if(!caseInsensitive)
                {
                    return string.Compare(leftValue, rightValue, StringComparison.OrdinalIgnoreCase) == 0;
                }

                return string.Compare(leftValue, rightValue) == 0;
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
