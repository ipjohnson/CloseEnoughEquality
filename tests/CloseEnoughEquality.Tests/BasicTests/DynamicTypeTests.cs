using CloseEnoughEquality.Tests.Classes.Basic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CloseEnoughEquality.Tests.BasicTests
{
    public class DynamicTypeTests
    {
        [Fact]
        public void CloseEnough_DynamicTestEqualObject_ReturnTrue()
        {
            var simpleObject = new SmallObject { IntValue = 5, DecimalValue = 2.0m, DoubleValue = 3.0, StringValue = "Hello" };

            dynamic expando = new ExpandoObject();

            expando.IntValue = simpleObject.IntValue;
            expando.DecimalValue = simpleObject.DecimalValue;
            expando.DoubleValue = simpleObject.DoubleValue;
            expando.StringValue = simpleObject.StringValue;

            Assert.True(CloseEnough.Equals(simpleObject, expando));
        }
    }

}
