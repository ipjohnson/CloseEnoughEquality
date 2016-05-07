using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public static class Extensions
    {
        public static string GetDiscrepancyString(this IReadOnlyList<ICloseEnoughDiscrepancy> discrepancies)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < discrepancies.Count; i++)
            {
                builder.Append(discrepancies[i]);

                if (i < discrepancies.Count - 1)
                {
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }
    }
}
