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
    public class DictionaryEqualityTest
    {
        [Fact]
        public void CloseEnough_SimpleObjectEqualDictionary_ReturnsTrue()
        {
            var simpleObject = new SmallObject { IntValue = 5, DecimalValue = 1.0m, DoubleValue = 2.0, StringValue = "Hello" };
            var dictionary = new Dictionary<string, object>
            {
                { "IntValue",simpleObject.IntValue },
                { "DecimalValue",simpleObject.DecimalValue },
                { "DoubleValue",simpleObject.DoubleValue },
                { "StringValue",simpleObject.StringValue },
            };

            CloseEnough.Equals(simpleObject, dictionary).Should().BeTrue();
        }


        [Fact]
        public void CloseEnough_SimpleObjectNotEqualDictionary_ReturnsFalse()
        {
            var simpleObject = new SmallObject { IntValue = 5, DecimalValue = 1.0m, DoubleValue = 2.0, StringValue = "Hello" };

            var dictionary = new Dictionary<string, object>
            {
                { "IntValue",10 },
                { "DecimalValue",simpleObject.DecimalValue },
                { "DoubleValue",simpleObject.DoubleValue },
                { "StringValue",simpleObject.StringValue },
            };

            CloseEnough.Equals(simpleObject, dictionary).Should().BeFalse();
        }
    }
}
