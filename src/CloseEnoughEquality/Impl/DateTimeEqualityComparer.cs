using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class DateTimeEqualityComparer : IEqualityWrapper
    {
        private ICloseEnoughConfiguration _configuration;

        public DateTimeEqualityComparer(ICloseEnoughConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool GeneratesDiscrepancy {  get { return false; } }

        public bool Applies(IPropertyInfo property)
        {
            return true;
        }

        public bool Equals(object left, object right, IPropertyInfo property)
        {
            bool allowTypeConversion = _configuration.AllowTypeConversion(property);

            try
            {
                DateTime leftValue;
                DateTime rightValue;

                if (allowTypeConversion)
                {
                    leftValue = (DateTime)Convert.ChangeType(left, typeof(DateTime));
                    rightValue = (DateTime)Convert.ChangeType(right, typeof(DateTime));
                }
                else
                {
                    leftValue = (DateTime)left;
                    rightValue = (DateTime)right;
                }

                switch(_configuration.DateTimeComparisonMode(property))
                {
                    case DateTimeComparisonMode.Exact:
                        return leftValue.Ticks == rightValue.Ticks;

                    case DateTimeComparisonMode.Millisecond:
                        return leftValue.Date == rightValue.Date &&
                               leftValue.Hour == rightValue.Hour &&
                               leftValue.Minute == rightValue.Minute &&
                               leftValue.Second == rightValue.Second &&
                               leftValue.Millisecond == rightValue.Millisecond;

                    case DateTimeComparisonMode.Second:
                        return leftValue.Date == rightValue.Date &&
                               leftValue.Hour == rightValue.Hour &&
                               leftValue.Minute == rightValue.Minute &&
                               leftValue.Second == rightValue.Second;

                    case DateTimeComparisonMode.Minute:
                        return leftValue.Date == rightValue.Date &&
                               leftValue.Hour == rightValue.Hour &&
                               leftValue.Minute == rightValue.Minute;

                    case DateTimeComparisonMode.Hour:
                        return leftValue.Date == rightValue.Date &&
                               leftValue.Hour == rightValue.Hour;

                    case DateTimeComparisonMode.Day:
                        return leftValue.Date == rightValue.Date;

                    case DateTimeComparisonMode.Month:
                        return leftValue.Year == rightValue.Year &&
                               leftValue.Month == rightValue.Month;

                    case DateTimeComparisonMode.Year:
                        return leftValue.Year == rightValue.Year;
                }
            }
            catch (Exception)
            {

            }

            return false;
        }    
    }
}
