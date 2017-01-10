using CloseEnoughEquality.Tests.Classes.Basic;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CloseEnoughEquality.Tests.BasicTests
{
    public class DictionaryEqualityTest
    {
        [Fact]
        public void CloseEnough_DictionaryEqualDictionary_RightSide_ReturnsTrue()
        {
            var simpleObject = new SmallObject { IntValue = 5, DecimalValue = 1.0m, DoubleValue = 2.0, StringValue = "Hello" };

            var dictionary = new Dictionary<string, object>
            {
                { "IntValue",simpleObject.IntValue },
                { "DecimalValue",simpleObject.DecimalValue },
                { "DoubleValue",simpleObject.DoubleValue },
                { "StringValue",simpleObject.StringValue },
            };
            var dictionary2 = new Dictionary<string, object>
            {
                { "IntValue",simpleObject.IntValue },
                { "DecimalValue",simpleObject.DecimalValue },
                { "DoubleValue",simpleObject.DoubleValue },
                { "StringValue",simpleObject.StringValue },
            };


            CloseEnough.Equals(dictionary, dictionary2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_DictionaryEqualReadOnlyDictionary_RightSide_ReturnsTrue()
        {
            var simpleObject = new SmallObject { IntValue = 5, DecimalValue = 1.0m, DoubleValue = 2.0, StringValue = "Hello" };

            var dictionary = new Dictionary<string, object>
            {
                { "IntValue",simpleObject.IntValue },
                { "DecimalValue",simpleObject.DecimalValue },
                { "DoubleValue",simpleObject.DoubleValue },
                { "StringValue",simpleObject.StringValue },
            };
            var readonlyDictionary = new ReadOnlyDictionary<string,object>( new Dictionary<string, object>
            {
                { "IntValue",simpleObject.IntValue },
                { "DecimalValue",simpleObject.DecimalValue },
                { "DoubleValue",simpleObject.DoubleValue },
                { "StringValue",simpleObject.StringValue },
            });
            
            CloseEnough.Equals(dictionary, readonlyDictionary).Should().BeTrue();
        }
        
        [Fact]
        public void CloseEnough_DictionaryEqualDictionary_RightSide_ReturnsFalse()
        {
            var simpleObject = new SmallObject { IntValue = 5, DecimalValue = 1.0m, DoubleValue = 2.0, StringValue = "Hello" };

            var dictionary = new Dictionary<string, object>
            {
                { "IntValue", simpleObject.IntValue },
                { "DecimalValue", simpleObject.DecimalValue },
                { "DoubleValue", simpleObject.DoubleValue },
                { "StringValue", simpleObject.StringValue },
            };
            var dictionary2 = new Dictionary<string, object>
            {
                { "IntValue", 10 },
                { "DecimalValue", simpleObject.DecimalValue },
                { "DoubleValue", simpleObject.DoubleValue },
                { "StringValue", simpleObject.StringValue },
            };

            CloseEnough.Equals(dictionary, dictionary2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_SimpleObjectEqualDictionary_RightSide_ReturnsTrue()
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
        public void CloseEnough_SimpleObjectNotEqualDictionary_RightSide_ReturnsFalse()
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

        [Fact]
        public void CloseEnough_SimpleObjectEqualDictionary_LeftSide_ReturnsTrue()
        {
            var simpleObject = new SmallObject { IntValue = 5, DecimalValue = 1.0m, DoubleValue = 2.0, StringValue = "Hello" };
            var dictionary = new Dictionary<string, object>
            {
                { "IntValue",simpleObject.IntValue },
                { "DecimalValue",simpleObject.DecimalValue },
                { "DoubleValue",simpleObject.DoubleValue },
                { "StringValue",simpleObject.StringValue },
            };

            CloseEnough.Equals(dictionary, simpleObject).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_SimpleObjectNotEqualDictionary_LeftSide_ReturnsFalse()
        {
            var simpleObject = new SmallObject { IntValue = 5, DecimalValue = 1.0m, DoubleValue = 2.0, StringValue = "Hello" };

            var dictionary = new Dictionary<string, object>
            {
                { "IntValue",10 },
                { "DecimalValue",simpleObject.DecimalValue },
                { "DoubleValue",simpleObject.DoubleValue },
                { "StringValue",simpleObject.StringValue },
            };

            CloseEnough.Equals(dictionary, simpleObject).Should().BeFalse();
        }
    }
}
