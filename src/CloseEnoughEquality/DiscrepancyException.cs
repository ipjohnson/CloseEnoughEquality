using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public class DiscrepancyException : Exception
    {
        public DiscrepancyException(Type type, IReadOnlyList<ICloseEnoughDiscrepancy> discrepancy)
            : base(string.Format("Discrepancies for type: {0}{1}{2}",
                   type.Name,
                   Environment.NewLine,
                   discrepancy.GetDiscrepancyString()))
        {
            Discrepancy = discrepancy;
        }

        public IReadOnlyList<ICloseEnoughDiscrepancy> Discrepancy { get; private set; }
    }
}
