using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CloseEnoughEquality.Tests.BasicTests
{
    public class DateTimeEqualityTests
    {
        [Fact]
        public void CloseEnough_DateTimeEqual_ReturnsTrue()
        {
            DateTime now = DateTime.Now;

            CloseEnough.Equals(now, now).Should().BeTrue();
        }


        [Fact]
        public void CloseEnough_DateTimeNotEqual_ReturnsFale()
        {
            DateTime now = DateTime.Now;

            CloseEnough.Equals(now, now.AddMilliseconds(1)).Should().BeFalse();
        }

        [Fact]
        public void CloseEnough_DateTimeCompareSameMillisecond_ReturnsTrue()
        {
            DateTime now = DateTime.Now;
            DateTime sameTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);

            CloseEnough.Equals(now, sameTime).Should().BeTrue();
        }


        [Fact]
        public void CloseEnough_DateTimeCompareExact_Returnsfalse()
        {
            DateTime now = DateTime.Now;
            DateTime sameTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);

            CloseEnough.Equals(now, sameTime, c => c.DateTimeComparisonMode(DateTimeComparisonMode.Exact)).Should().BeFalse();
        }

        [Fact]
        public void CloseEnough_DateTimeCompareDate_ReturnsTrue()
        {
            DateTime now = DateTime.Now;
            DateTime today = now.Date;

            CloseEnough.Equals(now, today, c => c.DateTimeComparisonMode(DateTimeComparisonMode.Day)).Should().BeTrue();
        }


        [Fact]
        public void CloseEnough_ObjectDateTimeCompareDate_ReturnsTrue()
        {
            var class1 = new { Date = DateTime.Now };
            var class2 = new { Date = class1.Date };
            
            CloseEnough.Equals(class1, class2, c => c.DateTimeComparisonMode(DateTimeComparisonMode.Day)).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_DateTimeCompareDate_ReturnsFalse()
        {
            DateTime now = DateTime.Now;
            DateTime today = now.AddDays(1);

            CloseEnough.Equals(now, today, c => c.DateTimeComparisonMode(DateTimeComparisonMode.Day)).Should().BeFalse();
        }
    }
}
