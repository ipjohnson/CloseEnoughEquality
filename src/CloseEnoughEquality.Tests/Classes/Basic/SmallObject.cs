using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Tests.Classes.Basic
{
    public class SmallObject
    {
        public decimal DecimalValue { get; internal set; }
        public double DoubleValue { get; internal set; }
        public int IntValue { get; internal set; }
        public string StringValue { get; internal set; }
    }
}
