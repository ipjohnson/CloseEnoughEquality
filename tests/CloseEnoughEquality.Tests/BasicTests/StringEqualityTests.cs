using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CloseEnoughEquality.Tests.BasicTests
{
    public class StringEqualityTests
    {
        [Fact]
        public void CloseEnough_ObjectStringEqual_ReturnTrue()
        {
            var class1 = new { StringValue = "Hello" };
            var class2 = new { StringValue = "Hello" };

            CloseEnough.Equals(class1, class2).Should().BeTrue(); ;
        }

        [Fact]
        public void CloseEnough_ObjectStringCaseInsensitiveEqual_ReturnTrue()
        {
            var class1 = new { StringValue = "Hello" };
            var class2 = new { StringValue = "HELLO" };

            CloseEnough.Equals(class1, class2, c => c.StringCaseSensitive(false)).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_ObjectMultipleStringNotEqual_ReturnFalse()
        {
            var class1 = new { StringValue = "Hello", SecondString = "Value" };
            var class2 = new { StringValue = "HELLO", SecondString = "VALUE" };

            CloseEnough.GetDiscrepancies(class1, class2, c => c.StringCaseSensitive(false, ForProperties.EndsWith("String"))).Should().HaveCount(1);
        }

        [Fact]
        public void CloseEnough_StringEmptyEqualNull_ReturnTrue()
        {
            var class1 = new { StringValue = "" };
            var class2 = new { StringValue = (string)null };

            CloseEnough.Equals(class1, class2).Should().BeTrue();
        }
        
        [Fact]
        public void CloseEnough_StringEmptyEqualNull_ReturnFalse()
        {
            var class1 = new { StringValue = "" };
            var class2 = new { StringValue = (string)null };

            CloseEnough.Equals(class1, class2, syntax => syntax.StringEmptyEqualToNull(false)).Should().BeFalse();
        }
    }
}
