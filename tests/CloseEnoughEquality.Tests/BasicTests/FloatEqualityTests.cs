using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloseEnoughEquality.Tests.Classes.Basic;
using Xunit;
using FluentAssertions;

namespace CloseEnoughEquality.Tests.BasicTests
{
    public class FloatEqualityTests
    {
        [Theory]
        [InlineData(5F, 5F, true)]
        [InlineData(5F, 10F, false)]
        [InlineData(0.3333333333333333F, (float)1 / 3, true)]
        public void CloseEnough_SimpleObject_TestFloatEquality(float floatValue1, float floatValue2, bool isEqual)
        {
            var simpleObject1 = new SimpleObject { FloatValue = floatValue1 };
            var simpleObject2 = new SimpleObject { FloatValue = floatValue2 };

            CloseEnough.Equals(simpleObject1, simpleObject2).Should().Be(isEqual);
        }

        [Fact]
        public void CloseEnoug_DoubleEpsilonEqual_ReturnsTrue()
        {
            var object1 = new { FloatValue = 3.0F, StringProp = "Hello" };
            var object2 = new { FloatValue = 2.99F, StringProp = "Hello" };

            Assert.True(CloseEnough.Equals(object1, object2, c => c.FloatEpsilon(0.01f)));
            Assert.True(CloseEnough.Equals(object1, object2, c => c.FloatEpsilon(0.01f, ForProperties.EndsWith("Value"))));
        }

        [Fact]
        public void CloseEnoug_DoubleEpsilonNotEqual_ReturnsFalse()
        {
            var object1 = new { FloatValue = 3.0F, StringProp = "Hello" };
            var object2 = new { FloatValue = 2.9F, StringProp = "Hello" };

            Assert.False(CloseEnough.Equals(object1, object2, c => c.FloatEpsilon(0.01F)));
            Assert.False(CloseEnough.Equals(object1, object2, c => c.FloatEpsilon(0.01F, ForProperties.EndsWith("Prop"))));
        }
    }
}
