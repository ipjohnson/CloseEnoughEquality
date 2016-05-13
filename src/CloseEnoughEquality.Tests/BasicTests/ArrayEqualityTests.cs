using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CloseEnoughEquality.Tests.BasicTests
{
    public class ArrayEqualityTests
    {
        [Fact]
        public void CloseEnough_ArrayEqual_ReturnTrue()
        {
            var array1 = new int[] { 1, 2, 3 };
            var array2 = new int[] { 1, 2, 3 };

            CloseEnough.Equals(array1, array2).Should().BeTrue();
        }


        [Fact]
        public void CloseEnough_ArrayNotEqual_ReturnFalse()
        {
            var array1 = new int[] { 1, 2, 3 };
            var array2 = new int[] { 1, 2, 4 };

            CloseEnough.Equals(array1, array2).Should().BeFalse();
        }

        [Fact]
        public void CloseEnough_MultiDimensionalArrayEqual_ReturnTrue()
        {
            var array1 = new int[3, 3] { { 0, 2, 4 }, { 1, 3, 5 }, { 2, 4, 6 } };
            var array2 = new int[3, 3] { { 0, 2, 4 }, { 1, 3, 5 }, { 2, 4, 6 } };

            CloseEnough.Equals(array1, array2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_MultiDimensionalArrayNotEqual_ReturnFalse()
        {
            var array1 = new int[3, 3] { { 0, 2, 4 }, { 1, 3, 5 }, { 2, 4, 6 } };
            var array2 = new int[3, 3] { { 0, 2, 8 }, { 1, 3, 5 }, { 2, 4, 6 } };

            CloseEnough.Equals(array1, array2).Should().BeFalse();
        }
    }
}
