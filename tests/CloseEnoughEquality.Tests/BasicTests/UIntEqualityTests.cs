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
    public class UIntEqualityTests
    {
        [Theory]
        [InlineData(5, 5, true)]
        [InlineData(5, 10, false)]
        public void CloseEnough_SimpleObject_TestUIntEquality(uint value1, uint value2, bool isEqual)
        {
            var simpleObject1 = new SimpleObject { UIntValue = value1 };
            var simpleObject2 = new SimpleObject { UIntValue = value2 };

            CloseEnough.Equals(simpleObject1, simpleObject2).Should().Be(isEqual);
        }
    }
}
