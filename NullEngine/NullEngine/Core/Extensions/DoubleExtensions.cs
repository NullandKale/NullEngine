﻿namespace NullEngine.Core.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Clamps the number between the number range.
        /// </summary>
        /// <param name="lowerBound">The min number of the range.</param>
        /// <param name="upperBound">The max number of the range.</param>
        /// <returns>A clamped number of the range.</returns>
        public static double Clamp(this double self, double lowerBound, double upperBound)
        {
            // Check lower bound.
            if (self < lowerBound)
            {
                self = lowerBound;
            }

            // Check upper bound.
            if (self > upperBound)
            {
                self = upperBound;
            }

            return self;
        }

        /// <summary>
        /// Normalizes a number from one number range to another.
        /// </summary>
        /// <param name="lowerBound">The min number of the current range.</param>
        /// <param name="upperBound">The max number of the current range.</param>
        /// <param name="minNum">The min number of the new range.</param>
        /// <param name="maxNum">The max number of the new range.</param>
        /// <param name="clamp">To clamp the number.</param>
        /// <returns>Returns the number normalized into the new range.</returns>
        public static double Normalize(this double self, double lowerBound, double upperBound, double minNum, double maxNum, bool clamp = false)
        {
            var num = self;

            // Clamp the number if requested.
            if (clamp)
            {
                num.Clamp(lowerBound, upperBound);
            }

            num = minNum + (num - lowerBound) * (maxNum - minNum) / (upperBound - lowerBound);

            return num;
        }

        /// <summary>
        /// Lerps the value to the value of the second number.
        /// </summary>
        /// <param name="value">The value to lerp to.</param>
        /// <param name="amount">The weight of the second value.</param>
        /// <returns>The lerped number.</returns>
        public static double Lerp(this double self, double value, double amount)
        {
            return self + (value - self) * amount;
        }
    }
}
