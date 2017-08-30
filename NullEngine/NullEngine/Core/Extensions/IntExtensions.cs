using System;

namespace NullEngine.Core.Extensions
{
    /// <summary>
    /// Extensions for the Int datatype.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Clamps the number between the number range.
        /// </summary>
        /// <param name="lowerBound">The min number of the range.</param>
        /// <param name="upperBound">The max number of the range.</param>
        /// <returns>A clamped number of the range.</returns>
        public static int Clamp(this int self, int lowerBound, int upperBound)
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
        public static int Normalize(this int self, int lowerBound, int upperBound, int minNum, int maxNum, bool clamp = false)
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
        public static int Lerp(this int self, int value, float amount)
        {
            return (int) Math.Floor(self + (value - self) * amount);
        }
    }
}
