using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Tests.Classes.Basic
{
    public class SimpleObject
    {
        public byte ByteValue { get; set; }

        public sbyte SByteValue { get; set; }

        public int IntValue { get; set; }

        public uint UIntValue { get; set; }

        public long LongValue { get; set; }

        public ulong ULongValue { get; set; }
 
        public string StringValue { get; set; }

        public double DoubleValue { get; set; }

        public decimal DecimalValue { get; set; }

        public DateTime DateTimeValue { get; set; }

        public TimeSpan TimeSpanValue { get; set; }
    }
}
