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
    public class IgnoreProperties
    {
        [Fact]
        public void CloseEnough_SkipProperty_ReturnTrue()
        {
            var simpleObject = new SimpleObject { IntValue = 5, DecimalValue = 5};
            var simpleObject2 = new SimpleObject { IntValue = 5, DecimalValue = 10 };

            CloseEnough.Equals(simpleObject, simpleObject2, c => c.SkipProperty(x => x.DecimalValue)).Should().BeTrue();
        }
        
        [Fact]
        public void CloseEnough_SkipProperties_ReturnTrue()
        {
            var simpleObject = new SimpleObject { IntValue = 5, DecimalValue = 5 };
            var simpleObject2 = new SimpleObject { IntValue = 5, DecimalValue = 10 };

            CloseEnough.Equals(simpleObject, simpleObject2, c => c.SkipPropertiesOfType<decimal>()).Should().BeTrue();
        }
        
        [Fact]
        public void CloseEnough_SkipPropertiesFilter_ReturnTrue()
        {
            var simpleObject = new SimpleObject { IntValue = 5, DecimalValue = 5 };
            var simpleObject2 = new SimpleObject { IntValue = 5, DecimalValue = 10 };

            CloseEnough.Equals(simpleObject, simpleObject2, c => c.SkipPropertiesOfType<object>(ForProperties.StartsWith("Decimal"))).Should().BeTrue();
        }
    }
}
