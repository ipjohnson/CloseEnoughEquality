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
    public class CircularReferenceTests
    {
        [Fact]
        public void CloseEnough_CircularReferenceEqual_ReturnTrue()
        {
            var class1 = new CircularReferenceClassA { ClassB = new CircularReferenceClassB { ClassC = new CircularReferenceClassC { IntValue = 5 } } };
            var class2 = new CircularReferenceClassA { ClassB = new CircularReferenceClassB { ClassC = new CircularReferenceClassC { IntValue = 5 } } };

            class1.ClassB.ClassC.ClassA = class1;
            class2.ClassB.ClassC.ClassA = class2;

            CloseEnough.Equals(class1, class2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_CircularReferenceNotEqual_ReturnFalse()
        {
            var class1 = new CircularReferenceClassA { ClassB = new CircularReferenceClassB { ClassC = new CircularReferenceClassC { IntValue = 5 } } };
            var class2 = new CircularReferenceClassA { ClassB = new CircularReferenceClassB { ClassC = new CircularReferenceClassC { IntValue = 10 } } };

            class1.ClassB.ClassC.ClassA = class1;
            class2.ClassB.ClassC.ClassA = class2;

            CloseEnough.Equals(class1, class2).Should().BeFalse();
        }
    }
}
