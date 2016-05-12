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
        
        [Fact]
        public void CloseEnoug_DoubleEpsilonEqual_ReturnsTrue()
        {
            var object1 = new { DoubleValue = 3.0, StringProp = "Hello" };
            var object2 = new { DoubleValue = 2.99, StringProp = "Hello" };

            Assert.True(CloseEnough.Equals(object1, object2, c => c.DoubleEpsilon(0.01)));
            Assert.True(CloseEnough.Equals(object1, object2, c => c.DoubleEpsilon(0.01, ForProperties.EndsWith("Value"))));
        }
        
        [Fact]
        public void CloseEnoug_DoubleEpsilonNotEqual_ReturnsFalse()
        {
            var object1 = new { DoubleValue = 3.0, StringProp = "Hello" };
            var object2 = new { DoubleValue = 2.9, StringProp = "Hello" };

            Assert.False(CloseEnough.Equals(object1, object2, c => c.DoubleEpsilon(0.01)));
            Assert.False(CloseEnough.Equals(object1, object2, c => c.DoubleEpsilon(0.01, ForProperties.EndsWith("Prop"))));
        }
    }
}
