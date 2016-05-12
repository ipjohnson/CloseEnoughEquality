using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public static class Extensions
    {
        /// <summary>
        /// Test that the object is close enough
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool IsCloseEnoughTo<TLeft,TRight>(this TLeft left, TRight right, Action<ICloseEnoughConfigurationSyntax<TLeft>> configuration = null)
        {
            return CloseEnough.Equals(left, right, configuration);
        }

        /// <summary>
        /// Gets a concatinated string of the discrepancies
        /// </summary>
        /// <param name="discrepancies">all discrepancies</param>
        /// <returns></returns>
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

        /// <summary>
        /// Is this an anonymous type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsAnonymousType(this Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetTypeInfo().GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }
    }
}
