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
    public class ByteEqualityTests
    {

        [Theory]
        [InlineData(5, 5, true)]
        [InlineData(5, 10, false)]
        public void CloseEnough_SimpleObject_TestByteEquality(byte value1, byte value2, bool isEqual)
        {
            var simpleObject1 = new SimpleObject { ByteValue = value1 };
            var simpleObject2 = new SimpleObject { ByteValue = value2 };

            CloseEnough.Equals(simpleObject1, simpleObject2).Should().Be(isEqual);
        }
    }
}
