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
    public class IntEqualityTests
    {
        [Theory]
        [InlineData(5, 5, true)]
        [InlineData(5, 10, false)]
        public void CloseEnough_SimpleObject_TestIntEquality(int intValue1, int intValue2, bool isEqual)
        {
            var simpleObject1 = new SimpleObject { IntValue = intValue1 };
            var simpleObject2 = new SimpleObject { IntValue = intValue2 };

            CloseEnough.Equals(simpleObject1, simpleObject2).Should().Be(isEqual);
        }
    }
}
