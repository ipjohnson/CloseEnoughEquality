using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality
{
    public interface ICloseEnoughDiscrepancy
    {
        string ToString();
    }

    public class ListCountDiscrepancy : ICloseEnoughDiscrepancy
    {
        public ListCountDiscrepancy(string path, int leftCount, int rightCount)
        {
            Path = path;
            LeftCount = leftCount;
            RightCount = rightCount;
        }

        public string Path { get; private set; }

        public int LeftCount { get; private set; }

        public int RightCount { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} count mismatch, Left: {1} Right: {2}", Path, LeftCount, RightCount);
        }
    }

    public class MissingPropertyDiscrepancy : ICloseEnoughDiscrepancy
    {
        public MissingPropertyDiscrepancy(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }

        public override string ToString()
        {
            return string.Format("Could not find property {0}", Path);
        }
    }

    public class ExtraPropertyDiscrepancy : ICloseEnoughDiscrepancy
    {
        public ExtraPropertyDiscrepancy(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }

        public override string ToString()
        {
            return string.Format("Extra property found at {0}", Path);
        }
    }

    public class CloseEnoughDiscrepancy : ICloseEnoughDiscrepancy
    {
        public CloseEnoughDiscrepancy(string path, object leftValue, object rightValue)
        {
            Path = path;
            LeftValue = leftValue;
            RightValue = rightValue;
        }

        public string Path { get; private set; }

        public object LeftValue { get; private set; }

        public object RightValue { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} not equal Left Side '{1}' Right Side '{2}' ", Path, LeftValue, RightValue);
        }
    }
}
