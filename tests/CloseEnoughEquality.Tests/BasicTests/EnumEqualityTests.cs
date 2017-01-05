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
    public class EnumEqualityTests
    {
        [Fact]
        public void CloseEnough_EnumClassEqual_ReturnTrue()
        {
            var class1 = new EnumClass { EnumValue = TestEnum.TestValue };
            var class2 = new EnumClass { EnumValue = TestEnum.TestValue };

            CloseEnough.Equals(class1, class2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_EnumClassNotEqual_ReturnFalse()
        {
            var class1 = new EnumClass { EnumValue = TestEnum.TestValue };
            var class2 = new EnumClass { EnumValue = TestEnum.TestValue2 };

            CloseEnough.Equals(class1, class2).Should().BeFalse();
        }

        [Fact]
        public void CloseEnough_EnumClassToDictionary_ReturnsTrue()
        {
            var class1 = new EnumClass { EnumValue = TestEnum.TestValue };
            var class2 = new Dictionary<string, object> { { "EnumValue", "TestValue" } };

            CloseEnough.Equals(class1, class2).Should().BeTrue();
        }
    }
}
