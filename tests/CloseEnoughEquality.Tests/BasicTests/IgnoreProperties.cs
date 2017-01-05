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

        [Fact]
        public void CloseEnough_CompareGenericEqual_ReturnTrue()
        {
            var generic1 = new GenericInterfacePropertyClass { InterfaceClass = new ImplementationClass<int> { TValue = 5 } };
            var generic2 = new GenericInterfacePropertyClass { InterfaceClass = new ImplementationClass<int> { TValue = 5 } };

            CloseEnough.Equals(generic1, generic2).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_CompareGenericNotEqual_ReturnFalse()
        {
            var generic1 = new GenericInterfacePropertyClass { InterfaceClass = new ImplementationClass<int> { TValue = 5 } };
            var generic2 = new GenericInterfacePropertyClass { InterfaceClass = new ImplementationClass<int> { TValue = 10 } };

            CloseEnough.Equals(generic1, generic2).Should().BeFalse();
        }

        [Fact]
        public void CloseEnough_SkipOpenGeneric_ReturnsTrue()
        {
            var generic1 = new GenericInterfacePropertyClass { InterfaceClass = new ImplementationClass<int> { TValue = 5 } };
            var generic2 = new GenericInterfacePropertyClass { InterfaceClass = new ImplementationClass<int> { TValue = 10 } };

            CloseEnough.Equals(generic1, generic2, c => c.SkipPropertiesOfType(typeof(IGenericInterface<>))).Should().BeTrue();
        }

        [Fact]
        public void CloseEnough_SkipOpenGenericImplementation_ReturnsTrue()
        {
            var generic1 = new GenericImplementationPropertyClass { InterfaceClass = new ImplementationClass<int> { TValue = 5 } };
            var generic2 = new GenericImplementationPropertyClass { InterfaceClass = new ImplementationClass<int> { TValue = 10 } };

            CloseEnough.Equals(generic1, generic2, c => c.SkipPropertiesOfType(typeof(IGenericInterface<>))).Should().BeTrue();
        }
    }
}
