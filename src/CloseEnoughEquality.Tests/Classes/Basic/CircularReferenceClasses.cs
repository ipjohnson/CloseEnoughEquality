using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Tests.Classes.Basic
{
    public class CircularReferenceClassA
    {
        public int IntValue { get; set; }

        public CircularReferenceClassB ClassB { get; set; }
    }

    public class CircularReferenceClassB
    {
        public int IntValue { get; set; }

        public CircularReferenceClassC ClassC { get; set;}
    }

    public class CircularReferenceClassC
    {
        public int IntValue { get; set; }

        public CircularReferenceClassA ClassA { get; set; }
    }
}
