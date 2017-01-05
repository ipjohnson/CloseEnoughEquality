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
    public class FieldEqualityTests
    {
        [Fact]
        public void CloseEnough_FieldEqual_ReturnTrue()
        {
            var class1 = new FieldClass { IntValue = 5, DoubleValue = 5.0 };
            var class2 = new FieldClass { IntValue = 5, DoubleValue = 5.0 };

            CloseEnough.Equals(class1, class2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_FieldNotEqual_ReturnFalse()
        {
            var class1 = new FieldClass { IntValue = 5, DoubleValue = 5.0 };
            var class2 = new FieldClass { IntValue = 6, DoubleValue = 5.0 };

            CloseEnough.Equals(class1, class2).Should().BeFalse();
        }

    }
}
