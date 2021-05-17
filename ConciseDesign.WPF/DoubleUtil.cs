using System;

namespace ConciseDesign.WPF
{
    public class DoubleUtil
    {
        public static bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
                return true;
            double num1 = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.22044604925031E-16;
            double num2 = value1 - value2;
            return -num1 < num2 && num1 > num2;
        }

        public static bool GreaterThan(double value1, double value2) => value1 > value2 && !DoubleUtil.AreClose(value1, value2);

        public static bool GreaterThanOrClose(double value1, double value2) => value1 > value2 || DoubleUtil.AreClose(value1, value2);
    }
}