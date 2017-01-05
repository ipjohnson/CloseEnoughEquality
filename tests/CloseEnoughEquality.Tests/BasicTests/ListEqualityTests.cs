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
    public class ListEqualityTests
    {
        [Fact]
        public void CloseEnough_EqualListType_ReturnTrue()
        {
            var listClass1 = new IntListClass { List = new List<int> { 0, 1, 2 } };
            var listClass2 = new IntListClass { List = new List<int> { 0, 1, 2 } };

            CloseEnough.Equals(listClass1, listClass2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_NotEqualListType_ReturnFalse()
        {
            var listClass1 = new IntListClass { List = new List<int> { 0, 1, 2 } };
            var listClass2 = new IntListClass { List = new List<int> { 0, 1, 3 } };

            CloseEnough.Equals(listClass1, listClass2).Should().BeFalse();
        }

        [Fact]
        public void CloseEnough_EqualComplexList_ReturnTrue()
        {
            var listClass1 = new SimpleObjectListClass { List = new List<SimpleObject> { new SimpleObject { IntValue = 3 } } };
            var listClass2 = new SimpleObjectListClass { List = new List<SimpleObject> { new SimpleObject { IntValue = 3 } } };

            CloseEnough.Equals(listClass1, listClass2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_NotEqualComplexList_ReturnFalse()
        {
            var listClass1 = new SimpleObjectListClass { List = new List<SimpleObject> { new SimpleObject { IntValue = 3 } } };
            var listClass2 = new SimpleObjectListClass { List = new List<SimpleObject> { new SimpleObject { IntValue = 5 } } };

            CloseEnough.Equals(listClass1, listClass2).Should().BeFalse();
        }
    }
}
