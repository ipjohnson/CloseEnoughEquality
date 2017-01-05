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
    public class DecimalEqualityTests
    {
        [Theory]
        [InlineData(5, 5, true)]
        [InlineData(5, 10, false)]
        public void CloseEnough_SimpleObject_TestDoubleEquality(decimal value1, decimal value2, bool isEqual)
        {
            var simpleObject1 = new SimpleObject { DecimalValue = value1 };
            var simpleObject2 = new SimpleObject { DecimalValue = value2 };

            CloseEnough.Equals(simpleObject1, simpleObject2).Should().Be(isEqual);
        }

        [Fact]
        public void CloseEnough_DecimalEpsilonEqual_ReturnTrue()
        {
            var object1 = new { DecimalValue = 3.0m, StringProp = "Hello" };
            var object2 = new { DecimalValue = 2.99m, StringProp = "Hello" };

            Assert.True(CloseEnough.Equals(object1, object2, c => c.DecimalEpsilon(0.01m)));
            Assert.True(CloseEnough.Equals(object1, object2, c => c.DecimalEpsilon(0.01m, ForProperties.EndsWith("Value"))));
        }
        
        [Fact]
        public void CloseEnough_DecimalEpsilonNotEqual_ReturnFalse()
        {
            var object1 = new { DecimalValue = 3.0m, StringProp = "Hello" };
            var object2 = new { DecimalValue = 2.9m, StringProp = "Hello" };

            Assert.False(CloseEnough.Equals(object1, object2, c => c.DecimalEpsilon(0.01m)));
        }
    }
}
