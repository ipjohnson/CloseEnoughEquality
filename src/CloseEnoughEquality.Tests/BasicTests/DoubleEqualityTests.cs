using CloseEnoughEquality.Tests.Classes.Basic;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CloseEnoughEquality.Tests.BasicTests
{
    public class DoubleEqualityTests
    {
        [Theory]
        [InlineData(5, 5, true)]
        [InlineData(5, 10, false)]
        [InlineData(0.3333333333333333, (double)1/3,true)]
        public void CloseEnough_SimpleObject_TestDoubleEquality(double doubleValue1, double doubleValue2, bool isEqual)
        {
            var simpleObject1 = new SimpleObject { DoubleValue = doubleValue1 };
            var simpleObject2 = new SimpleObject { DoubleValue = doubleValue2 };

            CloseEnough.Equals(simpleObject1, simpleObject2).Should().Be(isEqual);
        }        
    }
}
