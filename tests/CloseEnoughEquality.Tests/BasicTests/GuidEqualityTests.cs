using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CloseEnoughEquality.Tests.BasicTests
{
    public class GuidEqualityTests
    {
        [Fact]
        public void CloseEnough_GuidEqual_ReturnTrue()
        {
            var class1 = new { Guid = Guid.NewGuid() };
            var class2 = new { Guid = class1.Guid };

            CloseEnough.Equals(class1, class2).Should().BeTrue();
        }
        
        [Fact]
        public void CloseEnough_GuidNotEqual_ReturnFalse()
        {
            var class1 = new { Guid = Guid.NewGuid() };
            var class2 = new { Guid = Guid.NewGuid() };

            CloseEnough.Equals(class1, class2).Should().BeFalse();
        }
    }
}
